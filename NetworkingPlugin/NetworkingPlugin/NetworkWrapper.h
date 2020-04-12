#pragma once


#pragma pack(push, 1)

typedef struct MESSAGE
{
	int MessageID;
	float xPos;
	float yPos;
	int inputID;
	int inputStates[7];
	int playerID;
} Message;
#pragma pack(pop)

bool ConnectToIp(const char* ip);
bool StartUp(bool isServer);
int SendNetworkMessage(Message* messages[], int size);
void ReceivePackets(int& size, Message** output);
void DestroyInstances();
