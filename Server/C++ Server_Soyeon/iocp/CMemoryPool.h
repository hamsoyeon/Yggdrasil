#pragma once
template<class T1, int T2>
class CMemoryPool
{
public:
	CMemoryPool() {}

	void* operator new(size_t _size)
	{
		// �ʱ⿡ ���� �ʱ�ȭ.
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
		// m_ptr : new�ϸ� �ִ� �ּ�
		// _adr ������ �ּ�
		// ������ �ּ� �� next�� m_ptr������ �־��ְ�,
		// new�ϸ� �� m_ptr�� ������ ������ �־��ش�.

		memcpy(_adr, &m_ptr, sizeof(T1*));
		m_ptr = (unsigned char*)_adr;
	}

protected:
	static unsigned char* m_ptr;
};


template<class T1, int T2>
unsigned char* CMemoryPool<T1, T2>::m_ptr = nullptr;

