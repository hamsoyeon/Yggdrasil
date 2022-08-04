#pragma once

#define HOST_IP "127.0.0.1"
#define USER "root"
#define PASSWORD "1234"
#define DATABASE "Yggdrasil"
#define STR_SIZE 128

enum class SOC
{
	SOC_TRUE,
	SOC_FALSE,
};

enum class MAIN_PROTOCOL
{
	NONE,
	LOGIN,
	LOBBY,
	MAX
};

enum class DB_TYPE
{
	STAGE,
	END,
};

enum class GAMEOBJ_TYPE
{
	OBJ = 101,
	BOSS = 201,
	MOB = 301,
};

