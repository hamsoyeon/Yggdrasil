#pragma once

// �� ���� �ִ� ��ǥ��.
// �������� ���� ũ�⸦ �Ǵ��ϱ� ����,
// obj�� ��ġ���� ��ǥ���� ������.
#define MAX_X 15 
#define MAX_Y 15

#define MAX_SIZE 3

class CSession;
class CSector;

class CSectorMgr
{
public:
	static CSectorMgr* GetInst();
	static void Create();
	static void Destroy();

private:
	static CSectorMgr* m_instance;

	CSectorMgr();
	~CSectorMgr();

	// ������
public:
	// �÷��̾�,����,obj ... ���� ��Ŷ, <- ��ȣ�ۿ� ����
	// �÷��̾� ���� ��Ŷ
	// ���� ���� ��Ŷ
	// ������ ���� ��Ŷ
	void PlayerSendPacket(CSession* _ptr, int _protocol, bool _moveflag);
	void MonsterSendPacket(CSession* _ptr, int _protocol, bool _moveflag); // id�� �����ϸ� Ŭ�󿡼� id�� �����Ͽ� ���͸� �̵�.
	void ItemSendPacket(CSession* _ptr, int _protocol, bool _on_off_flag); // 

	void Init(); // Sector �ʱ�ȭ
	void AddSectorList(CSession* _ptr);
	void RemoveSectorList();

private:
	list<CSector*> m_sectorList;

};

// ������ ��ü�� ����.

// ĳ����, �� ���� ��Ŷó�� �Ŵ����� ����.
// �ϳ��� ������Ʈ���� ����.
// ���, ĳ����, ����ó�� ��Ŷ���� ����.
// �������� �Ŵ������� �����뿡 �и��ؼ� ����, ���������� ������� �� ������ ����, -> �÷��̾ ��Ŷó���� ���� �� ����..

// �ʱ� obj������ġ
// ��, ��, ����, �÷��̾ ������.
// ������ Ÿ���� ������ �ް�(������ �������� �������� ������ �� �� ����.)
// �� �� �ϳ��� ȣ��Ʈ�� ��Ƽ�, AI�� �� �ȿ����� ������, ���� �������� ���� �Ǿ��� �ϸ� ������ ȣ��Ʈ�� ������.
// ������ ���� ������ ���� ������ ������.
// ��ġ���� ������ ������ ������, ������ �̰��� �޾Ƽ� �� ������ ���� �Ѹ���.
// �ѷ��� ������ �������� �Ѵ�. // �׷� ���ü��� ���δ�.
// ���̶� �ο��� �̰�ٴ� ������ �Ѵ�. �׿��ٴ� ��Ŷ�� ������ ������ �����Ÿ� ������ ������Ŷ�� ������.
// ������ �ƴ� ������ ������ ������ �����̰� ����Ƿ� Ŭ���ʿ��� ������ ������ �ڵ带 �����ؾ���.
