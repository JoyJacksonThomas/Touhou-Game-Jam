#pragma once


#ifdef NETWORKINGPLUGIN_EXPORTS
#define NETWORKINGPLUGIN_SYMBOL __declspec(dllexport)
#else // !NETWORKPLUGIN_EXPORT
#ifdef NETWORKINGPLUGIN_IMPORT
#define NETWORKINGPLUGIN_SYMBOL __declspec(dllimport)
#else // !NETWORKPLUGIN_IMPORT
#define NETWORKINGPLUGIN_IMPORT 
#endif // NETWORKPLUGIN_IMPORT
#endif // NETWORKPLUGIN_EXPORT