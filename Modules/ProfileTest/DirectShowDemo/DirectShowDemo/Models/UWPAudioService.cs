﻿using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;
using Windows.Media.Render;
//using System;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Media.Devices;

namespace DirectShowDemo.Models
{
    class UWPAudioService
    {
        private const string OMENHeadset = "OMEN Mindframe";
        private AudioGraph uwpAudioGraph;
        //private AudioFileOutputNode fileOutputNode;//for File not use now.
        private AudioDeviceOutputNode deviceOutputNode;
        private AudioDeviceInputNode deviceInputNode;
        private DeviceInformationCollection outputDevices;

        private static UWPAudioService _instence;

        public static UWPAudioService Instence
        {
            get
            {
                if (_instence == null)
                {
                    _instence = new UWPAudioService();
                }
                return _instence;
            }
        }

        public async Task InitializeUWPAudio()
        {
            AudioGraphSettings settings = new AudioGraphSettings(AudioRenderCategory.Media);
            settings.QuantumSizeSelectionMode = QuantumSizeSelectionMode.LowestLatency;
            outputDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            foreach(DeviceInformation dev in outputDevices)
            {
                if (dev.Name.Contains(OMENHeadset))
                {
                    settings.PrimaryRenderDevice = dev;
                }
            }
            

            CreateAudioGraphResult result = await AudioGraph.CreateAsync(settings);

            if (result.Status != AudioGraphCreationStatus.Success)
            {
                // Cannot create graph
                return;
            }

            uwpAudioGraph = result.Graph;

            // Create a device output node
            CreateAudioDeviceOutputNodeResult deviceOutputNodeResult = await uwpAudioGraph.CreateDeviceOutputNodeAsync();
            if (deviceOutputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device output node
                return;
            }

            deviceOutputNode = deviceOutputNodeResult.DeviceOutputNode;

            // Create a device input node using the default audio input device
            CreateAudioDeviceInputNodeResult deviceInputNodeResult = await uwpAudioGraph.CreateDeviceInputNodeAsync(MediaCategory.Other);

            if (deviceInputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device input node
                return;
            }

            deviceInputNode = deviceInputNodeResult.DeviceInputNode;
#if false
            //For File recording not use now.
            FileSavePicker saveFilePicker = new FileSavePicker();
            saveFilePicker.FileTypeChoices.Add("Pulse Code Modulation", new List<string>() { ".wav" });
            saveFilePicker.FileTypeChoices.Add("Windows Media Audio", new List<string>() { ".wma" });
            saveFilePicker.FileTypeChoices.Add("MPEG Audio Layer-3", new List<string>() { ".mp3" });
            saveFilePicker.SuggestedFileName = "New Audio Track";
            StorageFile file = await saveFilePicker.PickSaveFileAsync();

            MediaEncodingProfile fileProfile = CreateMediaEncodingProfile(file);

            // Operate node at the graph format, but save file at the specified format
            CreateAudioFileOutputNodeResult fileOutputNodeResult = await uwpAudioGraph.CreateFileOutputNodeAsync(file, fileProfile);

            if (fileOutputNodeResult.Status != AudioFileNodeCreationStatus.Success)
            {
                // FileOutputNode creation failed
                //rootPage.NotifyUser(String.Format("Cannot create output file because {0}", fileOutputNodeResult.Status.ToString()), NotifyType.ErrorMessage);
                //fileButton.Background = new SolidColorBrush(Colors.Red);
                return;
            }
            fileOutputNode = fileOutputNodeResult.FileOutputNode;
            deviceInputNode.AddOutgoingConnection(fileOutputNode);
#endif

            // Connect the input node to both output nodes
            deviceInputNode.AddOutgoingConnection(deviceOutputNode);
        }

        public void StartAudio()
        {
            uwpAudioGraph.Start();
        }

        public void StopAudio()
        {
            uwpAudioGraph.Stop();
        }

        private MediaEncodingProfile CreateMediaEncodingProfile(StorageFile file)
        {
            switch (file.FileType.ToString().ToLowerInvariant())
            {
                case ".wma":
                    return MediaEncodingProfile.CreateWma(AudioEncodingQuality.High);
                case ".mp3":
                    return MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
                case ".wav":
                    return MediaEncodingProfile.CreateWav(AudioEncodingQuality.High);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
