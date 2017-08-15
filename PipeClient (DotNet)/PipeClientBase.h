#pragma once
#include "IoProtocolPipeClient.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::ComponentModel;
using namespace System::Threading;

#define DEFAULT_BUFFER_SIZE 256

namespace PipeClient {

	// ======redefine data structures in data.h for C#
	/*
	public enum class _PipeReceivedErrorCode : BYTE {
		ERROR_CAN_NOT_OPEN_USB_DEVICE = 0
	};
	*/
	
	public enum class PipeReceivedDataType : BYTE 
	{
		TYPE_ERROR = 0,
		TYPE_PERFORMANCE_DATA = 1,
	};
	
	[StructLayout(LayoutKind::Sequential)]
	public ref struct  PipeData : Object
	{
		public:
			int commandId;
			cli::array<BYTE>^ Data;
	};

	/*
	public value struct _PipeReceiveError
	{
		public:
			_PipeReceivedErrorCode ErrorCode;
	};
	*/

	//======================================================

	public delegate void PipeConnectedEventHandler(Object ^sender);
	public delegate void DataReceivedEventHandler(Object ^sender, PipeData ^rc);

	public ref class IoProtocolPipeClientWrapper
	{
	private:

		IoProtocolPipeClient *protocol;
		int bufferSize;

	public:
		IoProtocolPipeClientWrapper(System::String^ path)
		{
			IntPtr ptrToNativeString = Marshal::StringToHGlobalUni(path);
			this->protocol = new IoProtocolPipeClient(static_cast<WCHAR *>(ptrToNativeString.ToPointer()));
		}

		bool OpenDevice()
		{
			if (this->protocol)
			{
				return protocol->OpenDevice();
			}

			return false;
		}
		bool CloseDevice()
		{
			if (this->protocol)
			{
				return protocol->CloseDevice();
			}

			return false;
		}

		bool Read(PipeData ^data)
		{
			if (data == nullptr)
			{
				return false;
			}

			if (data->Data == nullptr)
			{
				return false;
			}

			bool result = false;
			unsigned char *buffer = new unsigned char[sizeof(int) + data->Data->Length]{ 0 };
			DWORD numberOfBytesRead = 0;

			if (this->protocol)
			{
				if (protocol->Read(buffer, sizeof(int) + data->Data->Length, &numberOfBytesRead))
				{
					data->commandId = *((int *)buffer);

					for (int i = sizeof(data->commandId), j = 0; i < data->Data->Length; i++, j++)
					{
						data->Data[j] = buffer[i];
					}

					result = true;
				}
			}

			if (buffer)
			{
				delete[] buffer;
			}

			return result;
		}

		bool Write(PipeData ^data)
		{
			if (data == nullptr)
			{
				return false;
			}

			bool result = false;
			unsigned char *buffer = NULL;
			DWORD numberOfBytesWrite = 0;

			if (this->protocol)
			{
				if (data->Data == nullptr)
				{
					data->Data = gcnew cli::array<BYTE>(this->bufferSize) { 0 };
				}

				buffer = new unsigned char[sizeof(int) + data->Data->Length];

				*((int*)buffer) = data->commandId;

				for (int i = sizeof(data->commandId), j = 0; i < sizeof(int) + data->Data->Length; i++, j++)
				{
					buffer[i] = data->Data[j];
				}

				result = protocol->Write(buffer, sizeof(int) + data->Data->Length, &numberOfBytesWrite);
			}

			if (buffer)
			{
				delete[] buffer;
			}
			return result;
		}

		void SetBufferSize(int size)
		{
			this->bufferSize = size;
		}
	};

	public ref class PipeClientBase
	{
		private :
			System::String^ pipepath;
			BackgroundWorker^ worker;
			int bufferSize;


		protected:
			//IoProtocolPipeClient *protocol = NULL;
			IoProtocolPipeClientWrapper ^protocol;

			PipeClientBase(System::String^ path) : bufferSize(DEFAULT_BUFFER_SIZE)
			{
				this->pipepath = path;
				this->worker = gcnew BackgroundWorker();

				this->protocol = gcnew IoProtocolPipeClientWrapper(path);
				this->protocol->SetBufferSize(this->bufferSize);
			}

		public:
			event PipeConnectedEventHandler ^PipeConnected;
			event DataReceivedEventHandler ^DataReceived;

			!PipeClientBase()
			{
				this->Disconnect();
			}

			
			bool Connect()
			{
				if (this->protocol)
				{
					if (this->protocol->OpenDevice())
					{
						PipeConnected(this);
						this->worker->DoWork += gcnew System::ComponentModel::DoWorkEventHandler(this, &PipeClient::PipeClientBase::OnDoWork);
						this->worker->WorkerSupportsCancellation = true;
						this->worker->WorkerReportsProgress = true;
						this->worker->ProgressChanged += gcnew System::ComponentModel::ProgressChangedEventHandler(this, &PipeClient::PipeClientBase::OnProgressChanged);
						if (this->worker)
						{
							this->worker->RunWorkerAsync();
						}
						
						return true;
					}
					else
					{
						
					}
				}

				return false;
			}
			
			bool Disconnect()
			{
				if (this->protocol)
				{
					if (this->worker != nullptr)
					{
						if (this->worker->IsBusy)
						{
							this->worker->CancelAsync();
						}
					}

					return this->protocol->CloseDevice();
				}


				return false;
			}

			void SetBufferSize(int size)
			{
				this->bufferSize = size;

				if (this->protocol)
				{
					this->protocol->SetBufferSize(this->bufferSize);
				}
			}

			virtual void OnDoWork(System::Object ^sender, System::ComponentModel::DoWorkEventArgs ^e) override;
			virtual void OnProgressChanged(System::Object ^sender, System::ComponentModel::ProgressChangedEventArgs ^e) override;
	};
}

