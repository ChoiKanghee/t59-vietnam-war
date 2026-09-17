# 314 — IMPLEMENTATION BRIEF M1A

## Truck Opening → Training Camp Arrival

**Nguồn thiết kế bắt buộc:** `314_LDD_01_Ngay_Nhan_Xe.md`  
**Engine:** Unity 6, 2D side-scrolling  
**Phạm vi:** Phần A–C đầu Màn 1  
**Mục tiêu:** Tạo một vertical slice hoàn chỉnh từ Main Menu lần đầu đến thời điểm người chơi điều khiển Lâm tại sân đón quân.

---

# 1. Yêu cầu cho Codex

Hãy đọc `AGENTS.md`, kiểm tra project, scene, package, input system và toàn bộ script/asset đang có trước khi chỉnh sửa.

Không được:

- Dựng lại Main Menu từ đầu.
- Xóa hoặc thay thế artwork menu xe 314 hiện tại.
- Ghi đè asset PNG nguồn.
- Làm hỏng wiring của sáu nút menu.
- Sửa `PlayerController` nếu không thật sự cần thiết.
- Tạo hệ thống song song khi project đã có abstraction tương đương.
- Dùng camera 3D, top-down hoặc chuyển động chiều sâu.
- Dùng nhạc có bản quyền hoặc đưa lời bài hát vào build.
- Tiếp tục sang tutorial AK, lựu đạn hoặc xe tăng trong milestone này.

Mọi thay đổi phải idempotent: chạy Editor command nhiều lần không nhân đôi GameObject hoặc event listener.

---

# 2. Hiện trạng đã biết

Project đã có các thành phần M0/M0A:

- `MainMenuController.cs`
- `MenuPanel.cs`
- `MenuSaveProvider.cs`
- `MainMenuSceneBuilder.cs`
- `MainMenuAssetPackApplier.cs`
- `MenuAmbience.cs`
- `MenuButtonVisual.cs`
- `MainMenu.unity` đã được tạo và wire.
- `Level01_Training` hiện là scene routing placeholder.
- Main Menu đang hiển thị xe tăng 314 tại doanh trại.
- `Continue` chưa có save provider thật nên đang disabled.
- Nhánh nền tảng từng được đẩy lên là `M1-player-foundation`; phải kiểm tra trạng thái repository thực tế trước khi làm.

Artwork menu xe tăng hiện tại sẽ được giữ làm:

- trạng thái menu sau khi hoàn thành Màn 1;
- fallback nếu truck opening chưa sẵn sàng;
- nền cho các trạng thái campaign sau.

---

# 3. Asset nguồn

Kiểm tra và dùng lại hai gói:

## B1

`314_TruckOpening_Asset_Pack_B_v1.zip`

Bao gồm:

- sky, núi xa, làng, đồng lúa, bìa rừng;
- looping road;
- tre/cỏ foreground;
- thân xe cam nhông tách bánh;
- bánh trước/sau;
- tài xế, tân binh ngồi;
- nông dân, trâu và props ven đường;
- dust frames/sheet;
- blank sign;
- preview và Unity docs.

## B2

`314_TruckOpening_Asset_Pack_B2_Training_Camp_Arrival_v1.1.zip`

Bao gồm:

- cổng doanh trại có chữ và không chữ;
- biển đỏ, sao vàng và text layer tách riêng;
- watchtower, fence;
- training yard midground;
- tents, crates, target row, trench và obstacle course;
- truck stop pad;
- Lâm xuống xe 4 frame;
- tân binh xuống xe 3 pose;
- instructor waiting;
- preview và Unity docs.

Ưu tiên asset cổng tách lớp và TextMeshPro. Chữ hiển thị:

```text
DOANH TRẠI
QUÂN ĐỘI NHÂN DÂN VIỆT NAM
```

Nếu các zip chưa được import vào `Assets/_Project/Art`, dừng và báo chính xác asset nào thiếu; không tự tạo placeholder thay thế artwork đã được sản xuất trừ khi chỉ dùng placeholder không phá hủy để kiểm thử logic.

---

# 4. Trạng thái Main Menu cần hỗ trợ

## Chưa có CampaignSave

- Hiển thị truck loop trên đường quê.
- Truck ở vùng phải/trung-phải.
- UI menu đọc rõ ở bên trái.
- Có thể dùng gradient tối nhẹ bên trái.
- Bánh xe quay, thân rung nhẹ, bụi lặp và parallax chạy vô hạn.

## Đã hoàn thành opening nhưng chưa hoàn thành Màn 1

- Continue tải `Level01_Training` tại `CP01_Arrival_LamGrounded` hoặc checkpoint mới hơn.
- Không phát lại truck opening.

## Đã hoàn thành Màn 1

- Dùng artwork xe tăng 314 tại doanh trại đang có.

Trong M1A chỉ cần triển khai đầy đủ hai trạng thái đầu và bảo toàn hook cho trạng thái sau Màn 1.

---

# 5. Luồng runtime bắt buộc

## A. Truck menu loop

1. Main Menu mở.
2. Nếu chưa có CampaignSave, dùng truck background state.
3. Truck và parallax lặp vô hạn khi người chơi ở menu/panel.
4. Mở Options/Journal/Level Select rồi Back không làm reset loop.
5. UI không bị ảnh hưởng bởi camera/parallax.

## B. Nhấn Start Journey

1. Khóa input menu và tránh double click.
2. Mờ dần menu UI.
3. Không hard cut sang một cảnh hình ảnh hoàn toàn khác.
4. Truck loop chuyển thành authored route.
5. Xe đi qua:
   - khu dân cư;
   - đồng lúa;
   - bìa rừng;
   - cổng doanh trại;
   - sân huấn luyện;
   - truck stop pad.
6. Tổng thời lượng từ click đến player control: khoảng 25–35 giây.

## C. Dừng xe và xuống xe

1. Trigger `ArrivalGateEntered` đổi ambience.
2. Trigger `TruckBrakeStart` bắt đầu giảm tốc.
3. Trigger `TruckStop` dừng xe đúng pad.
4. Các tân binh xuống trước.
5. Quang xuống ngay trước Lâm.
6. Lâm chạy animation xuống xe 4 frame.
7. Tại `LamGrounded`:
   - bật player input;
   - camera chuyển sang follow Lâm nếu cần;
   - ghi `opening_truck_completed = true`;
   - ghi checkpoint `CP01_Arrival_LamGrounded`;
   - autosave;
   - mở objective đầu tiên.

Không dùng cinematic camera hoặc close-up.

---

# 6. Hội thoại M1A

Sau khi Quang và Lâm xuống xe:

> **Quang:** “Cậu cũng về đơn vị tăng à?”  
> **Lâm:** “Ừ. Pháo thủ.”  
> **Quang:** “Tôi là Quang, nạp đạn. Vậy chắc còn gặp nhau dài.”

Objective sau hội thoại:

```text
Theo nhóm đến điểm tập trung
```

Không hard-code câu tiếng Việt trực tiếp trong MonoBehaviour. Dùng key ổn định, ví dụ:

```text
mission01.dialogue.quang.intro_01
mission01.dialogue.lam.intro_01
mission01.dialogue.quang.intro_02
mission01.objective.reach_assembly
```

Nếu Unity Localization chưa được cài, không tự ý thêm package mà không ghi rõ. Có thể tạo data asset/key-based interface đủ để chuyển sang String Table sau, nhưng không được rải literal text trong logic gameplay.

---

# 7. Audio

M1A cần các hook:

- truck engine loop;
- wheel/road loop;
- countryside ambience;
- forest/camp ambience;
- brake/stop;
- footsteps/disembark;
- optional radio source.

Radio hiện chỉ dùng placeholder hoặc để trống AudioClip:

```text
RADIO_TRACK_MARCH_PLACEHOLDER
```

Không import “Tiến quân ca”, “Đoàn Vệ quốc quân”, bản ghi thương mại hoặc lời bài hát vào project trong milestone này.

Radio phải có khả năng nhỏ dần khi vào bìa rừng/cổng doanh trại.

---

# 8. Save tối thiểu cho M1A

Trước khi code, tìm save system hiện có.

Nếu chưa có, triển khai tối thiểu nhưng có đường mở rộng:

```text
CampaignSave
- schemaVersion
- currentScene
- checkpointId
- openingTruckCompleted
- mission01Completed
- lastSavedUtc
```

Yêu cầu:

- Lưu dưới `Application.persistentDataPath`.
- Ghi file tạm rồi thay file chính để giảm nguy cơ corrupt.
- Có một backup gần nhất.
- `GlobalSettings` tách khỏi `CampaignSave`.
- `MenuSaveProvider` hoặc adapter của nó báo đúng trạng thái Continue.
- Continue load scene/checkpoint mà không phát lại opening.
- Nếu app đóng trước `LamGrounded`, lần sau phát lại opening.
- Nếu app đóng sau autosave CP01, Continue bắt đầu ở sân đón quân.
- Không tạo manual save giữa sequence.

## New Journey

Nếu hiện đã có save và người chơi chọn Start Journey/New Journey:

- hiển thị modal cảnh báo;
- Cancel không đổi dữ liệu;
- Confirm xóa `CampaignSave`, không xóa `GlobalSettings`;
- truck menu trở lại rồi tự chạy opening;
- không bắt bấm Start lần hai.

Nếu modal chưa nằm trong scope thực tế của scene hiện tại, giữ API/hook và ghi rõ phần còn thiếu; không âm thầm xóa save.

---

# 9. Kiến trúc gợi ý

Ưu tiên state machine/scripted sequence thay vì một cutscene đóng kín.

Có thể dùng các thành phần tương đương:

```text
MainMenuCampaignVisualState
OpeningTruckController
OpeningRouteDirector
ParallaxLayer2D
TruckVisualRig
DisembarkSequence
Mission01CheckpointService
CampaignSaveService
CampaignMenuSaveProvider
LocalizedDialogueRunner
```

Tên class có thể thay đổi để phù hợp convention hiện có; không tạo duplicate nếu project đã có hệ thống tương đương.

State gợi ý:

```text
MenuLoop
→ MenuFade
→ VillageRoute
→ RiceFieldRoute
→ ForestRoute
→ GateRoute
→ Braking
→ RecruitsDisembark
→ LamDisembark
→ PlayerControl
```

Sequence phải có thể skip nhanh trong Editor/dev build bằng một dev-only control, nhưng không hiển thị skip trong player build ở milestone này.

---

# 10. Scene và Editor tooling

Ưu tiên thêm một Editor command dưới menu hiện có:

```text
314 → M1 → Build/Update M1A Truck Opening
```

Command nên:

- tìm scene/objects hiện có;
- tạo phần còn thiếu;
- reuse object theo tên/marker ổn định;
- wire serialized references;
- cấu hình sorting layer, Canvas và camera;
- không ghi đè object do người dùng tạo nếu không thuộc ownership của command;
- không tự save scene ngoài scene M1A/MainMenu được yêu cầu;
- chạy lại không sinh duplicate.

Nếu cần scene riêng cho authored route, giải thích rõ cách chuyển scene mà vẫn tạo cảm giác liền mạch. Ưu tiên kiến trúc ít scene hơn nếu không làm MainMenu khó bảo trì.

---

# 11. Camera và parallax

- Orthographic 2D.
- Không xoay camera.
- Không zoom điện ảnh mạnh.
- Menu loop và route dùng cùng framing/art direction.
- Background cover được 16:9 và không lộ viền ở aspect ratio phổ biến.
- CanvasScaler giữ reference 1920×1080, Scale With Screen Size, Match 0.5.
- Parallax layer không drift tạo seam rõ.
- Foreground không che UI hoặc Lâm khi xuống xe.

---

# 12. Player handoff

Trước `LamGrounded`:

- PlayerController disabled hoặc input map gameplay bị khóa.
- Không thể nhảy khỏi xe hoặc di chuyển nhân vật vô hình.

Sau `LamGrounded`:

- Lâm ở đúng ground layer.
- Rigidbody/Collider hợp lệ.
- Input map gameplay được bật đúng một lần.
- Camera follow đúng target.
- Không còn script sequence ghi đè transform của Lâm.
- Objective “Theo nhóm đến điểm tập trung” hiện lên.

Milestone M1A kết thúc tại đây. Không cần dựng briefing/cấp AK trong lượt này.

---

# 13. Acceptance tests

## Main Menu

- [ ] New profile hiển thị truck loop, không hiển thị tank menu.
- [ ] Menu text đọc rõ ở 1920×1080, 16:10 và 21:9.
- [ ] Mở/đóng panel không reset animation.
- [ ] Start Journey chỉ chạy một lần dù double click.

## Opening route

- [ ] UI fade và truck tiếp tục chuyển động, không hard cut khó chịu.
- [ ] Đủ village → rice field → forest → gate → yard.
- [ ] Route dài khoảng 25–35 giây.
- [ ] Wheel, body, dust và parallax không trượt sai pivot.
- [ ] Trigger theo đúng thứ tự.

## Arrival

- [ ] Truck dừng đúng stop pad.
- [ ] Tân binh và Lâm không xuyên xe/mặt đất.
- [ ] Quang xuống trước Lâm.
- [ ] Hội thoại text xuất hiện đúng thứ tự.
- [ ] Player input chỉ bật tại `LamGrounded`.
- [ ] Objective đầu tiên xuất hiện.

## Save/Continue

- [ ] Thoát trước `LamGrounded` → chạy lại opening.
- [ ] Thoát sau `LamGrounded` → Continue tại CP01.
- [ ] Continue enabled khi save hợp lệ.
- [ ] Save hỏng thử backup hoặc báo lỗi an toàn.
- [ ] GlobalSettings không bị xóa khi New Journey.

## Regression

- [ ] Artwork tank menu cũ vẫn còn.
- [ ] Sáu nút menu và panel Back/Escape vẫn hoạt động.
- [ ] Không sửa ngoài scope hoặc mất asset meta.
- [ ] Không có Console error.
- [ ] Editor command chạy lại không sinh duplicate.

---

# 14. Deliverables bắt buộc

Sau khi hoàn thành, báo cáo:

1. Danh sách file tạo mới.
2. Danh sách file đã sửa và lý do.
3. Scene/prefab/asset nào được wire.
4. Editor command cần chạy.
5. Build Profile/Scene List cần chỉnh gì.
6. Cách test New Journey, Continue và CP01.
7. Những bước đã test trong Unity và những bước chưa test.
8. Asset nào bị thiếu hoặc chỉ dùng placeholder.
9. Không tuyên bố runtime-tested nếu chưa thật sự chạy Play Mode.

---

# 15. Definition of Done M1A

M1A hoàn thành khi người dùng có thể:

1. Mở game lần đầu và thấy xe cam nhông chạy trên đường quê.
2. Dùng Main Menu bình thường.
3. Nhấn Bắt đầu hành trình.
4. Xem UI biến mất và chiếc xe tiếp tục hành trình tới doanh trại.
5. Thấy tân binh, Quang và Lâm xuống xe trong camera 2D ngang.
6. Đọc đoạn hội thoại giới thiệu Lâm–Quang.
7. Nhận quyền điều khiển Lâm tại đúng marker.
8. Thấy objective đầu tiên.
9. Thoát game, mở lại và Continue từ CP01 mà không xem lại opening.

Không triển khai các khu D–K cho đến khi chín mục trên chạy ổn định.

