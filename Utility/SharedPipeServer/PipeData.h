#pragma once
#define DEFAULT_BUFFER_SIZE 256

typedef struct
{
	int iCommandId;
	unsigned char *data;
} PipeData, *LPPipeData;