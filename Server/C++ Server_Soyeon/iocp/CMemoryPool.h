#pragma once
template<class T1, int T2>
class CMemoryPool
{
public:
	CMemoryPool() {}

	void* operator new(size_t _size)
	{
		// 초기에 전부 초기화.
		if (m_ptr == nullptr)
		{
			m_ptr = (unsigned char*)malloc(_size * T2);
			unsigned char* current = m_ptr;
			unsigned char* next = m_ptr;

			for (int i = 0; i < T2 - 1; i++)
			{
				next += sizeof(T1);
				memcpy(current, &next, sizeof(T1*));
				current = next;
			}
			memset(current, 0, sizeof(T1*));
		}

		unsigned char* returnPtr = m_ptr;
		memcpy(&m_ptr, m_ptr, sizeof(T1*));

		return returnPtr;
	}

	void operator delete(void* _adr)
	{
		// m_ptr : new하면 주는 주소
		// _adr 삭제할 주소
		// 삭제할 주소 앞 next에 m_ptr번지를 넣어주고,
		// new하면 줄 m_ptr을 삭제할 번지를 넣어준다.

		memcpy(_adr, &m_ptr, sizeof(T1*));
		m_ptr = (unsigned char*)_adr;
	}

protected:
	static unsigned char* m_ptr;
};


template<class T1, int T2>
unsigned char* CMemoryPool<T1, T2>::m_ptr = nullptr;

