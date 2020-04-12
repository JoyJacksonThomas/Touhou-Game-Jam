#include "NetworkPlugin.h"

#include "NetworkWrapper.h"
int Test()
{
	return 2;
}


int Connect(const char* ip)
{
	return ConnectToIp(ip);	
}

int StartupNetwork(int isServer)
{
	if (StartUp(isServer))
		return 1;
	else
		return 0;
	
}

int SendGameMessages(Message* messages[], int size)
{
	return SendNetworkMessage(messages, size);
}


void GetPacketsFromPeer(int& size, Message** messages)
{
	return ReceivePackets( size, messages);
}

void ExitNetworking()
{
	DestroyInstances();
}