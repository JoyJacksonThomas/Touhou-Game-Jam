#pragma once

class NetworkManager
{
private:
	NetworkManager() { isServer = false; raknetPeer = nullptr; };
	~NetworkManager() {
		//delete connectedIP;
		//connectedIP = nullptr;
	};
	static NetworkManager* Instance;
	

public:

	bool isServer;
	void* raknetPeer;
	void* connectedIP;


	static NetworkManager* Get() 
	{
		if (!Instance)
			Instance = new NetworkManager();
		return Instance;
	}

	static void Destroy()
	{
		if (Instance)
		{	
			delete Instance;
			Instance = nullptr;
		}
		
	}

	void SetAsServer(bool isThisServer) { isServer = isThisServer; };

};

