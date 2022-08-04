#pragma once
#pragma comment(lib,"ws2_32")
#pragma comment(lib,"libmysql.lib")

#include <Winsock2.h>
#include <stdio.h>
#include <iostream>
#include <time.h>
#include <list>
#include <queue>
#include <fstream>
#include <assert.h>
using std::ofstream;
using std::ifstream;
using std::list;
using std::queue;
using std::vector;

#include <map>
using std::map;
using std::make_pair;

#include <string>
using std::wstring;
using std::string;

#include <unordered_set>
using std::unordered_set;

#include <locale.h>
#include <tchar.h>

#include <mysql.h>

#include "func.h"
#include "define.h"
#include "struct.h"

// #define new new( _NORMAL_BLOCK, __FILE__,__LINE__)

