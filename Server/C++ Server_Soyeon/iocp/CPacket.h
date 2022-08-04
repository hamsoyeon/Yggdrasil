#pragma once
#include "CSocket.h"
class CPacket :public CSocket
{
public:
	CPacket()
	{
		m_s_packNo = 0;
		m_r_packNo = 0;
	}
	CPacket(SOCKET _sock) :CSocket(_sock)
	{
		m_s_packNo = 0;
		m_r_packNo = 0;
	}
	// 패킹, 언패킹 기능을 명시적으로 
	void Packing(unsigned long _protocol, const BYTE* _data, int _size = 0);
	void UnPacking(int& _protocol, BYTE* _buf);
	void UnPacking(int& _protocol);
	void UnPacking(BYTE* _data);

private:
	int m_s_packNo;
	int m_r_packNo;

};

namespace Serialize
{
	class CSerialize
	{
	public:
		CSerialize(const UINT32& _size);
		void Set(BYTE* _buf, const UINT32 _size);
		CSerialize(const CSerialize& copy) {
			this->Set(copy.m_buf, copy.m_size);
		}
		void operator=(const CSerialize& copy) {
			this->Set(copy.m_buf, copy.m_size);
		}

		UINT32 GetBufSize() const { return m_size; }
		UINT32 GetReadDataSize() const { return m_readPtr; }
		UINT32 GetWriteDataSize() const { return m_writePtr; }
		const BYTE* GetBuf() { return m_buf; }

	public:
		void Serialize(const string& in);
		template<typename T>
		void Serialize(const vector<T>& in);
		template<typename T>
		void Serialize(const T& in);
		template<typename T>
		void DeSerialize(T* out);
		template<typename T>
		void DeSerialize(vector<T>* out);
		void DeSerialize(string* out);

	public:
		CSerialize& operator << (const UINT8& in);
		CSerialize& operator << (const UINT16& in);
		CSerialize& operator << (const UINT32& in);
		CSerialize& operator << (const UINT64& in);
		CSerialize& operator << (const INT8& in);

		CSerialize& operator >> (UINT8* out);
		CSerialize& operator >> (UINT16* out);
		CSerialize& operator >> (UINT32* out);
		CSerialize& operator >> (UINT64* out);
		CSerialize& operator >> (INT8* out);
	private:
		BOOL CheckWriteBoundary(UINT32 offsetSize_);
		BOOL CheckReadBoundary(UINT32 offsetSize_);

		template <typename T>
		void WRITE(const T& data);
		template <typename T>
		void READ(T* pData);

	private:
		UINT32 m_size = 0;
		UINT32 m_readPtr = 0;
		UINT32 m_writePtr = 0;
		BYTE* m_buf = nullptr;
	};

	template<typename T>
	inline void CSerialize::Serialize(const vector<T>& in)
	{
		size_t size = in.size();
		WRITE(size);
		for (int i = 0; i < size; i++) {
			WRITE(in[i]);
		}
	}
	template<typename T>
	inline void CSerialize::Serialize(const T& in)
	{
		WRITE(in);
	}
	template<typename T>
	inline void CSerialize::DeSerialize(T* out)
	{
		READ(out);
	}
	template<typename T>
	inline void CSerialize::DeSerialize(vector<T>* out)
	{
		int size = 0;
		READ(&size);

		for (int i = 0; i < size; i++) {
			T data;
			READ(&data);
			out->push_back(data);
		}
	}
	template<typename T>
	inline void CSerialize::WRITE(const T& data)
	{
		int dataSize = sizeof(T);
		if (CheckWriteBoundary(dataSize)) {
			memcpy_s((void* const)(m_buf + m_writePtr), sizeof(m_buf), (const void*)(&data),
				dataSize);
			m_writePtr += dataSize;
		}
		else {
			assert(FALSE);
		}
	}
	template<typename T>
	inline void CSerialize::READ(T* pData)
	{
		int dataSize = sizeof(T);
		if (CheckReadBoundary(dataSize)) {
			memcpy_s((void* const)(pData), dataSize, (const void* const)(m_buf + m_readPtr), dataSize);
			m_readPtr += dataSize;
		}
		else {
			assert(FALSE);
		}
	}
}