#pragma once


#include "Lib.h"

#include "NetworkWrapper.h"

#ifdef  __cplusplus
extern "C"
{
#else //  !__cplusplus

#endif //  __cplusplus

NETWORKINGPLUGIN_SYMBOL int Test();

NETWORKINGPLUGIN_SYMBOL int Connect(const char* ip);
	
NETWORKINGPLUGIN_SYMBOL int StartupNetwork(int isServer);

NETWORKINGPLUGIN_SYMBOL int SendGameMessages(Message* messages[], int size);

NETWORKINGPLUGIN_SYMBOL void GetPacketsFromPeer(int& size, Message** messages);

NETWORKINGPLUGIN_SYMBOL void ExitNetworking();

#ifdef  __cplusplus
}
#else //  !__cplusplus

#endif //  __cplusplus
