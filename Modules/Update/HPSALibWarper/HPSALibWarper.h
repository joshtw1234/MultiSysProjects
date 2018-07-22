#pragma once


/*
Class for C++ language.
*/
class __declspec(dllexport) HPSALibWarper
{
public:
	HPSALibWarper();
	~HPSALibWarper();
	double GetHPSALastAnalysis();
	int GetOMENUpdate(int rIdx, int * uPri);
	bool IsHPSARunning();
	bool HPSARunAnalysis();
private:
};

