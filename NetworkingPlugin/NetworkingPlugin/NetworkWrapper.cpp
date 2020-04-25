#include "NetworkWrapper.h"
#include "NetworkManager.h"

#include "RakPeerInterface.h"
#include "RakNetTypes.h"  // MessageID
#include "MessageIdentifiers.h"
#include <stdio.h>
#include <string>
#include <vector>
#include <array>

enum GameMessages
{
	ID_CONNECTED_TO_SERVER = ID_USER_PACKET_ENUM + 1,
	MOVE_CURSOR
};


NetworkManager* NetworkManager::Instance = NULL;


bool StartUp(bool isServer)
{
	RakNet::RakPeerInterface* peer = RakNet::RakPeerInterface::GetInstance();
	if (isServer)
	{
	RakNet::SocketDescriptor sd(60000, 0);
	peer->Startup(2, &sd, 1);
	peer->SetMaximumIncomingConnections(2);
	}
	else
	{
	RakNet::SocketDescriptor sd;
	peer->Startup(1, &sd, 1);
	}
	NetworkManager::Get()->SetAsServer(isServer);
	NetworkManager::Get()->raknetPeer = peer;
	return 1;
}

bool ConnectToIp(const char* ip)
{
	RakNet::RakPeerInterface* peer = (RakNet::RakPeerInterface*)NetworkManager::Get()->raknetPeer;
	if (NetworkManager::Get()->isServer)
	{
		NetworkManager::Get()->connectedIP = &peer->GetMyBoundAddress();
		return 1;
	}
	else
	{		
		RakNet::ConnectionAttemptResult res = peer->Connect(ip, 60000, 0, 0);	

		while (res != RakNet::ConnectionAttemptResult::CONNECTION_ATTEMPT_ALREADY_IN_PROGRESS || res != RakNet::ConnectionAttemptResult::CONNECTION_ATTEMPT_STARTED)
		{
			return 0;
		}


		return 1;
	}
}

int SendNetworkMessage(Message* messages[], int size)
{
	try {
		Message* outM = (Message*)*messages;
	RakNet::RakPeerInterface* peer = (RakNet::RakPeerInterface*)NetworkManager::Get()->raknetPeer;
	for(int i = 0; i < size; i++)
	{
		Message sendMessage;
		sendMessage.MessageID = outM[i].MessageID;
		sendMessage.inputID = outM[i].inputID;
		for (int j = 0; j < 7; j++)
		{
			sendMessage.inputStates[j] = outM[i].inputStates[j];
		}
		sendMessage.playerID = outM[i].playerID;
		sendMessage.xPos = outM[i].xPos;
		sendMessage.yPos = outM[i].yPos;

		RakNet::SystemAddress* ip = (RakNet::SystemAddress*)NetworkManager::Get()->connectedIP;	//Something is going funky wunky here		
		peer->Send(reinterpret_cast<char*>(&sendMessage), sizeof(sendMessage), HIGH_PRIORITY, RELIABLE_ORDERED, 0, *ip, NetworkManager::Get()->isServer);
		
	}
	}
	catch (...)
	{
		return 0;
	}

	return 1;
}



void ReceivePackets(int& size, Message** output)
{
	
	RakNet::RakPeerInterface* peer = (RakNet::RakPeerInterface*)NetworkManager::Get()->raknetPeer;
	RakNet::Packet* packet;

	std::vector<Message> messages;
	Message nMessage;
	int count = 0;
	nMessage.MessageID = 0;

	messages.push_back(nMessage);

	count++;
	
	for (packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive())
	{
		switch (packet->data[0])
		{
		case ID_REMOTE_NEW_INCOMING_CONNECTION:
		{
			nMessage.MessageID = ID_REMOTE_NEW_INCOMING_CONNECTION;

			messages.push_back(nMessage);
		}
		break;
		case ID_CONNECTION_REQUEST_ACCEPTED:
		{
			nMessage.MessageID = ID_CONNECTION_REQUEST_ACCEPTED;

			messages.push_back(nMessage);
			NetworkManager::Get()->connectedIP = &packet->systemAddress;
		}
		break;
		case ID_NEW_INCOMING_CONNECTION:
		{
			nMessage.MessageID = ID_NEW_INCOMING_CONNECTION;

			messages.push_back(nMessage);

		}
		break;
		case MOVE_CURSOR:
		{ 
			try {

			Message receivedMessage = *(Message*)(packet->data);			  
			messages.push_back(receivedMessage);
			}
			catch(...)
			{
				nMessage.MessageID = 137;

				messages.push_back(nMessage);
			}
		}
		break;
		default:
			{
			try {

				Message receivedMessage = *(Message*)(packet->data);
				messages.push_back(receivedMessage);
			}
			catch (...)
			{
				
				nMessage.MessageID = packet->data[0];

				messages.push_back(nMessage);
			}
			

			
			break;
			}
		}
		count++;

	}
	size = messages.size();
	*output = new Message[messages.size()];
	for (int i = 0; i < messages.size(); i++)
	{
		(*output)[i] = messages[i];
	}
	messages.clear();
	//std::copy(messages.begin(), messages.end(), *output);
	return;
}

void DestroyInstances()
{
	if (NetworkManager::Get()->raknetPeer != nullptr )
	{
		RakNet::RakPeerInterface* peer = (RakNet::RakPeerInterface*)NetworkManager::Get()->raknetPeer;
		RakNet::RakPeerInterface::DestroyInstance(peer);
	}
		NetworkManager::Destroy();
	return;
}