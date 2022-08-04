#include "pch.h"
#include "CInGameDB.h"

CInGameDB::CInGameDB()
{

}

CInGameDB::CInGameDB(MYSQL _sql)
	:CDB(_sql)
{
	const char* query;
	char temp[100];
	ZeroMemory(temp, 100);
	sprintf(temp, "select * from stage_01");
	query = temp;

	if (mysql_query(&m_mysql, (char*)query))
	{
		printf("** %s **\n", mysql_error(&m_mysql));
	}
	m_sql_result = mysql_store_result(&m_mysql);

	while ((m_sql_row = mysql_fetch_row(m_sql_result)) != NULL)
	{
		TCHAR Name[STR_SIZE]; ZeroMemory(Name, sizeof(Name));
		int kind, id; float fx, fy, fz;

		// 멀티바이트를 유니코드로 변환한다.
		MultiByteToWideChar(CP_ACP, 0, m_sql_row[0], strlen(m_sql_row[0]), Name, strlen(m_sql_row[0]));

		t_GameObj obj;

		// 유니코드로 변환된 멀티바이트를 복사시킨다.
		memcpy(obj.m_charName, Name, strlen(m_sql_row[0]) * 2);
		obj.m_kind = atoi(m_sql_row[1]);
		obj.m_objID = atoi(m_sql_row[2]);
		obj.m_objPos.x = atof(m_sql_row[3]);
		obj.m_objPos.y = atof(m_sql_row[4]);
		obj.m_objPos.z = atof(m_sql_row[5]);

		m_stageData.push_back(obj); // 첫번째 스테이지에 데이터를 넣는다.

		//_wsetlocale(LC_ALL, _T("korean"));
		//_tprintf(L"%s\n", data.m_charName);
		//_tprintf(L"%d\n", data.m_objID);
		//_tprintf(L"%f\n", data.m_objPos.x);
		//_tprintf(L"%f\n", data.m_objPos.y);
		//_tprintf(L"%f\n", data.m_objPos.z);
	}

	mysql_free_result(m_sql_result);
}

