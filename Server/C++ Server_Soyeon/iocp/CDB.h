#pragma once
class CDB
{
public:
	CDB() {}
	CDB(MYSQL _sql) :m_mysql(_sql) {}
	virtual ~CDB() {}
protected:
	MYSQL m_mysql;
	MYSQL_RES* m_sql_result;
	MYSQL_ROW m_sql_row;
	MYSQL_STMT* m_stmt_set;

};

