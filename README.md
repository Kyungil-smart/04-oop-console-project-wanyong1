[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=22157947&assignment_repo_type=AssignmentRepo)
🎮 콘솔 게임 프로젝트
제목 - 아무튼 RPG


<br>

🕹️ 조작 방법

이동 : ↑ ↓ ← →

선택/결정 : Enter

인벤토리 열기/닫기 : I

NPC 대화 : F

공격 : SpaceBar

재시작 : R

<br>
📌 게임 방식

플레이어는 숲(미로) 맵을 이동하며 NPC를 만나고, 몬스터를 피하거나 처치하면서 출구를 향해 나아간다.

이동할 때마다 STEP(이동 횟수) 가 감소하며, STEP이 0이 되면 스테이지가 리셋된다.

HP(체력)와 MP(마나)는 화면 하단 HUD로 표시된다.

SpaceBar 공격은 주변 8칸 범위 공격이며, MP가 없으면 공격이 제한된다.

<br>
🧩 게임 구성
❤️ HP / 💙 MP HUD

화면 아래쪽에 HP/MP 게이지가 표시된다.

HP가 0이 되면 해당 스테이지가 재시작된다.

MP는 스킬/공격 행동에 사용되며 부족하면 행동이 제한된다. 

PlayerCharacter

<br>
🧱 벽 (Wall)

맵 테두리는 벽으로 구성되어 있으며 통과할 수 없다. 

ForestScene

<br>
□ 밀기 블록 (Push Block)

□ 블록은 플레이어가 밀 수 있다.

밀고자 하는 방향의 다음 칸이 벽/다른 오브젝트면 밀 수 없다.

가시(Spike) 위로는 블록이 올라갈 수 없다.

<br>
^ 가시 (Spike)

플레이어가 가시 타일을 밟으면 피해를 입는다.

가시는 FloorObject로 배치되어 있으며, 타일 위에 올라서면 상호작용이 발생한다.

<br>
👤 NPC 대화

NPC는 플레이어와 인접한 4방향에서 F 키로 대화할 수 있다.

대화가 진행되는 동안 플레이어 조작은 잠시 비활성화된다.

특정 NPC와의 대화가 끝나면 출구가 활성화된다.

<br>
☆ / ★ 출구 (Exit)

☆ : 비활성 출구 (아직 나갈 수 없음)

★ : 활성 출구 (해당 타일로 이동하면 다음 스테이지로 이동)

출구는 ExitDevice로 구현되어 있으며, 활성화되면 심볼이 변경된다.

<br>
⚔️ 전투 / 공격 방식
✴️ 기본 공격 (SpaceBar)

SpaceBar를 누르면 플레이어 주변 8칸(상/하/좌/우/대각선) 을 동시에 타격한다.

공격 시 잠깐 * 이 표시되며(0.2초), 대상이 IDamageable이면 데미지를 준다.

🧟 몬스터

몬스터는 맵에 스폰되며, 플레이어는 몬스터가 있는 칸으로 이동할 수 없다.

공격으로 몬스터를 처치할 수 있다.

<br>
🧪 아이템
🔮 신비한 부적 (Mystic Amulet)

인벤토리에서 사용할 수 있는 아이템이다.

사용 시 인벤토리에서 제거되며 “신비한 힘이 느껴진다...” 메시지가 출력된다. 

MysticAmulet

<br>
🔁 재시작 조건

HP가 0이 되면 자동으로 스테이지가 재시작된다.

STEP이 0이 되면 스테이지가 재시작된다.

R 키를 누르면 즉시 재시작한다.
