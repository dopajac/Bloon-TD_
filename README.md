# Bloon-TD_
<img width="475" height="310" alt="image" src="https://github.com/user-attachments/assets/8b1ccf06-17ca-4510-a08c-0824f704ace3" />

메이플 소스를 기반으로 한 타워 디펜스입니다. 

Unity 2D를 기반으로 하여 제작 하였습니다.

---

## 주요 기능

### 유저(캐릭터) 시스템

#### 유저 소환 시스템

마우스 클릭 위치를 Grid 기반으로 스냅하여 정확한 타일 배치

UI 위 클릭 방지 기능 포함

자원(meso) 소모하여 유닛 생성

#### 유저 정보 UI 

유닛 클릭 시 프로필/레벨/공격력/경험치 등을 실시간 표시

EXP 슬라이더 반영

Upgrade 버튼 활성 여부 자동 판별

#### 유저 레벨링 & 경험치 시스템

스테이지 경험치를 기반으로 자동 레벨업

공격력 / 최대 경험치 자동 재계산

#### 직업 시스템

직업 레벨(JobLevel)에 따라 1~3차 전직 가능

직업별로 전직 가능한 UI 패널 자동 활성화

특정 ID(excludedIds)의 유닛은 전직 제한

전직 시 기존 능력치/레벨 유지한 채 새로운 클래스 프리팹으로 교체

### 전투 시스템

#### 1) 단일 타겟 자동 공격

감지 범위 내 첫 번째 몬스터를 큐(Queue) 기반으로 타겟팅

BulletController를 사용하여 투사체 발사

몬스터 사망 시 자동으로 큐에서 제거

공격 속도는 유저 능력치 기반

#### 2) 광역 근접 공격

CircleCollider 범위 내 모든 몬스터에게 동시 피해

InvokeRepeating 기반 주기적 공격

각 몬스터마다 Hit Effect 생성

이펙트 및 타격 애니메이션 동기화

#### 3) 지속형 광역 공격

유저와 몬스터 사이에 Ray 효과 스킬 생성

빔 범위 안에 있는 모든 적 지속 타격

공격 속도에 따른 코루틴 기반 지속 피해

몬스터 죽거나 범위 벗어나면 자동 종료

### 몬스터 시스템

#### 몬스터 스폰

지정된 spawnDelay 간격으로 자동 생성

최대 생성 수 도달 시 웨이브 종료 후 다음 Level로 진입

생성된 몬스터들은 List로 관리

랜덤 위치 스폰

#### 몬스터 이동

Unity Spline 기반 경로 자동 이동

경로 끝점(finishobj)에 도달 시 life 감소 후 몬스터 비활성화

#### 몬스터 생명/사망 처리

HP 관리 및 피격 처리

사망 시 애니메이션 후 오브젝트 풀링 방식으로 비활성화

메소/경험치 지급

#### 몬스터 리스폰

‘못 잡은 몬스터’를 버튼 클릭으로 재소환

startPosition에서 다시 이동 시작

사망 이벤트(OnMonsterDeath)로 공격 스크립트와 자동 연동

----
## 기술스택

#### Game Engine

Unity 2D (2022~ 버전)

#### Gameplay & System

C# (MonoBehaviour 기반 스크립트)

Physics2D: Collider, Raycast, OverlapCircle

Coroutine 기반 타이머, 지속 공격 구현

Event 기반 구조 (OnMonsterDeath)

오브젝트 풀링 방식 몬스터 재활성화

#### 몬스터 경로

SplineAnimate (Unity Spline 패키지)
