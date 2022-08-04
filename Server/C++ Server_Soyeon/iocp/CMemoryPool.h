#pragma once

template<class T>
class CMemoryPool
{
public:
	CMemoryPool(SOCKET _sock) {}
	CMemoryPool() {}

	void* operator new(size_t _size) {
		// 초기에 전부 초기화.
		static byte* bt = nullptr;
		static int len = 0;

		if (m_ptr == nullptr)
		{
			m_ptr = malloc(_size * 3);
			bt = (byte*)m_ptr;
			//reinterpret_cast<T*>(bt)->m_next = (T*)(bt + len);
			bt = bt + len;

			len += _size;
			m_next = bt + len;
			//reinterpret_cast<T*>(bt)->m_next = (T*)m_next;
			//reinterpret_cast<T*>(bt)->m_id = 1234;
			//memcpy(m_next, bt, sizeof(int));

		}
		else
		{
			bt = bt + len;
			len += _size;
			m_next = bt + len;
		}

		return (void*)bt;
	}

	//void* operator delete(void* _adr)
	//{

	//}

	void* Call()
	{
		void* ptr = m_ptr;
		memcpy(ptr, m_ptr, sizeof(int));
		return m_ptr;
	}

protected:
	// null일때만 4byte를 리턴.
	static void* m_ptr;

	static void* m_next;
};

template<class T>
void* CMemoryPool<T>::m_ptr = nullptr;

template<class T>
void* CMemoryPool<T>::m_next = nullptr;
