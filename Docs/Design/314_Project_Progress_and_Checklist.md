# 314 — Project Progress, Decisions & Handoff Checklist

> **Last updated:** 2026-09-18  
> **Purpose:** Tài liệu handoff duy nhất để tiếp tục dự án ở chat/Codex session khác mà không mất bối cảnh, quyết định đã chốt hoặc tiến độ thực tế.  
> **Important:** Phân biệt rõ ba trạng thái: **DONE & MERGED**, **IN PROGRESS**, và **NOT STARTED**. Không suy diễn rằng asset đã import đồng nghĩa tính năng đã được triển khai.

---

## 1. Project identity

- **Tên game:** 314
- **Repository:** `https://github.com/ChoiKanghee/t59-vietnam-war`
- **Engine:** Unity `6000.3.24f1`
- **Thể loại:** 2D side-scrolling, kết hợp gameplay bộ binh và xe tăng.
- **Chủ đề:** Hành trình hư cấu trong bối cảnh chiến tranh Việt Nam, tập trung vào Lâm và kíp xe tăng Type 59 số hiệu 314.
- **Ngôn ngữ sản xuất hiện tại:** Tiếng Việt.
- **Localization dự kiến:** Việt/Anh/Nhật, làm sau khi luồng gameplay chính ổn định.
- **Cách làm việc:**
  - ChatGPT trong cuộc trò chuyện này chỉ review, lập kế hoạch, chia scope, viết prompt và hướng dẫn thao tác.
  - Code được thực hiện bởi Codex trong VS Code sau khi người dùng tự dán prompt.
  - Không tự động sửa repo, commit, push hoặc merge từ chat review.
  - Mọi file mới cần đưa vào repo phải được thông báo rõ trước khi prompt Codex yêu cầu sử dụng.

---

## 2. Design sources already in repository

Hai tài liệu thiết kế đã được thêm vào repo:

- `Docs/Design/314_M1A_Implementation_Brief.md`
- `Docs/Design/314_LDD_01_Ngay_Nhan_Xe.md`

Vai trò:

- `314_LDD_01_Ngay_Nhan_Xe.md`: nguồn thiết kế đầy đủ cho Màn 1, câu chuyện, gameplay, luồng menu và acceptance criteria.
- `314_M1A_Implementation_Brief.md`: brief kỹ thuật cho vertical slice từ Main Menu truck opening đến lúc trao quyền điều khiển Lâm tại sân đón quân.
- File hiện tại là **progress/handoff tracker**, không thay thế LDD; nếu có khác biệt về thiết kế, LDD là nguồn thiết kế chính, còn trạng thái triển khai phải theo file progress này và Git hiện tại.

---

## 3. High-level game direction already agreed

### 3.1 Campaign/menu progression

Main Menu thay đổi theo tiến trình campaign:

1. **Chưa bắt đầu:** xe cam nhông chạy trên đường quê.
2. **Sau khi nhận xe 314 / hoàn thành Màn 1:** xe tăng 314 tại doanh trại.
3. **Giữa campaign:** xe 314 bám bùn, có hư hỏng và đồ dùng tích lũy.
4. **Hoàn thành game:** xe 314 đứng yên trong ánh sáng buổi sáng, nhấn mạnh hòa bình.

### 3.2 Màn 1 — “Ngày nhận xe”

Luồng A–K đã chốt:

| Mục | Nội dung | Trạng thái triển khai |
|---|---|---|
| A | Main Menu xe cam nhông chạy vô hạn | Gần hoàn thành; còn merge fix RuralLife |
| B | Đường tới thao trường: radio, trò chuyện, làng → đồng → rừng → cổng | Chưa làm |
| C | Xuống xe, tập hợp, trình diện sĩ quan | Chưa làm |
| D | Hướng dẫn di chuyển và vượt chướng ngại vật | Chưa làm |
| E | Huấn luyện AK, semi/full-auto, nhả cò ngắn | Chưa làm |
| F | Cover, chiến hào, lựu đạn | Chưa làm |
| G | Huấn luyện MG cố định và B41 | Chưa làm |
| H | Gặp kíp xe 314 | Chưa làm |
| I | Lái xe tăng và bắn pháo/MG | Chưa làm |
| J | Bài kiểm tra tổng hợp | Chưa làm |
| K | Kết màn/loading bằng thư hoặc nhật ký | Chưa làm |

Mục tiêu thời lượng Màn 1: khoảng 18–20 phút.

### 3.3 Nhân vật/kíp xe chính

- **Lâm:** nhân vật chính.
- **Khánh:** chỉ huy xe.
- **Hoàng:** lái xe.
- **Quang:** pháo thủ.
- **Sơn:** chỉ huy tổ bộ binh liên quan.

Tên/vai trò chi tiết vẫn có thể được điều chỉnh trong LDD, nhưng không tự thay đổi bằng prompt code.

---

## 4. Asset inventory and repository locations

### 4.1 B1 — Truck Opening Asset Pack

Đã import vào:

`Assets/_Project/Art/TruckOpening/314_TruckOpening_Asset_Pack_B`

Nội dung chính:

- B01 sky.
- B02 mountains.
- B03 village.
- B04 rice field.
- B05 looping dirt road.
- B06 foreground bamboo/grass.
- B07 forest transition.
- B08 truck body without wheels.
- B09 front wheel.
- B10 rear wheel.
- B11 recruits.
- B12 driver.
- Farmers, buffalo and rural props.
- Dust animation frames and sheet.
- Blank sign and documentation/preview files.

Mục đích: Main Menu lần đầu và phần đầu tuyến đường tới doanh trại.

### 4.2 B2 — Training Camp Arrival

Đã import vào:

`Assets/_Project/Art/TruckOpening/314_TruckOpening_Asset_Pack_B2_Training_Camp_Arrival`

Nội dung chính:

- Cổng doanh trại và biển tên.
- Watchtower/fence.
- Training yard.
- Truck stop pad.
- Training props/facilities.
- Lâm xuống xe.
- Tân binh xuống xe.
- Sĩ quan chờ.

**Important:** B2 mới chỉ được import và normalize importer. B2 arrival runtime chưa được triển khai.

---

## 5. Git history and merged state

### 5.1 Main branch

Pull Request `#1` từ `M1A-truck-opening` vào `main` đã được merge.

PR chứa năm nhóm commit:

1. `feat(m0): add main menu foundation`
2. `feat(m0a): integrate main menu asset pack`
3. `feat(m0a): update menu`
4. `chore(m1a): import truck opening and camp arrival assets`
5. `feat(m1a): add main menu truck opening loop`

Khoảng 292 files trong PR là hợp lý vì bao gồm asset B1/B2, Unity `.meta`, menu foundation và M1A Phase 1.

### 5.2 Current follow-up branch

Branch đã được tạo để sửa phần rural life bị bỏ sót:

`fix/m1a-rural-life`

Trạng thái cuối cùng được quan sát:

- Codex đã thêm nông dân và trâu vào Main Menu.
- Unity screenshot xác nhận chúng đã hiển thị.
- Chưa có bằng chứng cuối cùng rằng fix đã được commit, push hoặc merge.
- Khi quay lại dự án phải kiểm tra Git trước, không giả định branch sạch.

Lệnh kiểm tra đầu tiên:

```powershell
git branch --show-current
git status --short
git diff --name-only
```

---

## 6. Implemented systems — DONE & MERGED

### 6.1 M0 Main Menu foundation

Các phần nền tảng đã được tạo và merge:

- `MainMenuController`
- `MenuPanel`
- `MenuSaveProvider`
- `MainMenuSceneBuilder`
- Scene `MainMenu.unity`
- Sáu nút menu và panel wiring cơ bản.
- `Continue` giữ disabled khi chưa có save provider thực tế.

### 6.2 M0A visual menu

Đã có:

- `MenuAmbience`
- `MenuButtonVisual`
- `MainMenuAssetPackApplier`
- Artwork menu xe tăng 314 tại doanh trại.

Artwork xe tăng cũ phải tiếp tục được giữ làm:

- fallback nếu truck presentation không sẵn sàng;
- trạng thái menu sau khi hoàn thành Màn 1;
- nền cho các trạng thái campaign tương lai.

### 6.3 M1A Phase 1 truck loop

Các file chính đã merge:

- `Assets/_Project/Editor/M1ATruckOpeningBuilder.cs`
- `Assets/_Project/Scripts/Opening/OpeningTruckController.cs`
- `Assets/_Project/Scripts/Opening/ParallaxLayer2D.cs`
- `Assets/_Project/Scripts/Opening/TruckVisualRig.cs`
- `Assets/_Project/Scripts/UI/MainMenu/MainMenuCampaignVisualState.cs`
- `Assets/_Project/Prefabs/Opening/TruckOpeningPresentation.prefab`
- cập nhật `Assets/_Project/Scenes/MainMenu.unity`

Chức năng đã có:

- Truck presentation world-space độc lập với UI menu.
- Parallax deterministic dựa trên travel distance, tránh cumulative drift.
- Wheel rotation theo chiều kim đồng hồ.
- Body bob nhẹ.
- Dust animation dùng tám frame rời ở khoảng 11 FPS.
- Menu/panel có thể mở đóng mà không chủ động reset truck loop.
- Development selector giữa `TruckOpening` và `ExistingTankFallback`.
- Builder Unity idempotent theo thiết kế.
- Importer normalization cho 58 PNG production B1/B2, loại trừ Preview.
- Không sửa byte PNG.
- Sky đã đổi thành một sprite tĩnh để loại bỏ seam.

Unity menu command:

`314 → M1A → Build or Update Truck Opening Menu`

---

## 7. Approved visual values — MUST PRESERVE

Các giá trị dưới đây đã được người dùng chỉnh trực tiếp trong Unity và yêu cầu builder phải tái tạo đúng. Không được “tối ưu”, reset hoặc thay đổi khi làm task khác.

| Object | Local position | Scale | Sorting order | Ghi chú |
|---|---:|---:|---:|---|
| TruckBody | builder-approved current value | `0.65` trong bản dựng đã thấy | `3` | Body nằm sau bánh |
| FrontWheel | `(3.94, -1.90, 0)` | `0.21` | `4` | Đã căn lại đúng hốc bánh |
| RearWheel | `(-3.12, -1.98, 0)` | `0.20` | `4` | Đã căn lại đúng hốc bánh |
| DustEmitter | `(-4.67, -2.49, 0)` | `0.34` | `5` | Nằm sau đuôi xe và trên body |

### B06 foreground

- Đã chỉnh lại để giữ cả tán tre phía trên và foliage phía dưới.
- Foreground có thể đi ngang và tạm thời che một phần xe; đây là hiệu ứng chiều sâu đã chấp nhận.
- Không được quay về transform cũ làm mất tán tre.

### Sky

- Chỉ có `Sky/Tile 1`.
- Position `(0, 0, 0)`.
- Scale `(1, 1, 1)`.
- Sorting order `-60`.
- Không có `ParallaxLayer2D`.
- Không nằm trong runtime parallax list.
- Không tạo Tile 2/Tile 3 và không dùng Flip X để giả seamless.

---

## 8. RuralLife fix — IN PROGRESS

### 8.1 Reason for fix

LDD yêu cầu Main Menu loop có:

- đồng lúa;
- nông dân;
- trâu;
- mái nhà;
- núi xa;
- cây và bụi tiền cảnh.

M1A Phase 1 ban đầu chỉ import/normalize farmers và buffalo nhưng không đặt chúng vào presentation. Đây là scope omission, không phải quyết định chuyển chúng sang phần sau.

### 8.2 Current implementation observed

Codex đã được yêu cầu tạo layer `RuralLife` và dùng các sprite production rời:

- `Characters/farmer_01_holding_rice.png`
- `Characters/farmer_02_harvesting.png`
- `Characters/farmer_03_carrying_baskets.png`
- `Characters/water_buffalo_01.png`

Không dùng/slice `B13_farmers_buffalo_sheet.png`.

Screenshot cuối xác nhận:

- nông dân và trâu đã xuất hiện trong vùng đồng ruộng;
- tỷ lệ nhỏ hơn truck;
- nằm sau truck và sau B06 foreground;
- không chiếm trọng tâm;
- menu vẫn đọc được.

### 8.3 Checklist phải hoàn thành trước khi merge fix

- [ ] Xác nhận đang ở branch `fix/m1a-rural-life`.
- [ ] Play Mode ít nhất 30–60 giây.
- [ ] Nông dân/trâu loop không teleport khó chịu.
- [ ] Không chồng đôi sau nhiều vòng.
- [ ] Không có khoảng trống bất thường quá lâu.
- [ ] Mở/đóng Options, Journal, Level Select không reset loop.
- [ ] Console có 0 compilation/runtime error.
- [ ] Thoát Play Mode.
- [ ] Chạy builder lần một.
- [ ] Chạy builder lần hai.
- [ ] Xác nhận đúng một `RuralLife` layer.
- [ ] Xác nhận không duplicate tile roots/sprites.
- [ ] Xác nhận Sky/wheels/dust/B06 không bị reset.
- [ ] Review `git status --short` và `git diff --name-only`.
- [ ] Commit fix.
- [ ] Push branch.
- [ ] Tạo PR vào `main`.
- [ ] Merge sau khi review Files changed.

Gợi ý commit message:

```text
fix(m1a): add rural life to truck menu loop
```

---

## 9. Explicitly NOT IMPLEMENTED

Không được báo cáo các mục dưới đây là đã hoàn thành:

### 9.1 Start Journey transition

Chưa triển khai sequence mới gồm:

- khóa input và chống double-click;
- fade/hide menu UI;
- truck tiếp tục chạy sau khi UI biến mất;
- chuyển menu loop sang authored route;
- fade/transition kỹ thuật nếu cần.

Cho đến khi task mới được làm, nút Start Journey vẫn giữ behavior cũ/direct scene-load hiện có.

### 9.2 Authored route to camp

Chưa triển khai:

- khu dân cư;
- đồng lúa theo tuyến;
- bìa rừng;
- cổng doanh trại;
- sân huấn luyện;
- truck stop pad;
- sequence dài khoảng 25–35 giây.

### 9.3 B2 arrival

Chưa triển khai runtime:

- truck dừng;
- Lâm xuống xe;
- recruits disembark;
- instructor/officer waiting;
- player handoff tại `LamGrounded`.

### 9.4 Save/campaign state

Chưa triển khai đầy đủ:

- `CampaignSaveManager`;
- adapter thật cho `MenuSaveProvider`;
- `opening_truck_completed`;
- checkpoint `CP01_Arrival_LamGrounded`;
- Continue tải checkpoint;
- New Journey confirmation/delete flow;
- menu state sau `mission01_completed`.

### 9.5 Localization

Chưa thêm Unity Localization package hoặc String Tables.

Định hướng đã chốt:

- nội dung UI/dialogue/objective/loading phải chuyển sang key-based String Table về sau;
- Việt là ngôn ngữ gốc;
- Anh và Nhật làm sau;
- chưa triển khai lúc usage thấp hoặc khi M1 gameplay chưa ổn định.

### 9.6 Remaining Màn 1 gameplay

Movement training, AK, cover, grenade, fixed MG, B41, gặp kíp xe, tank driving/gunnery, combined test và ending/loading chưa bắt đầu.

---

## 10. Protected scope / files that must remain untouched unless a future task explicitly authorizes them

Trong M1A Phase 1 và RuralLife fix không được thay đổi:

- `PlayerController` và player foundation hiện có.
- Dialogue system.
- Save system ngoài hook được yêu cầu.
- `Level01_Training`.
- Packages.
- Project Settings.
- Input Actions.
- Build Settings/Build Profiles.
- Existing menu button listeners.
- Existing tank fallback hierarchy.
- PNG source bytes.

Các thay đổi về những phần này cần task/branch riêng và prompt ghi rõ phạm vi.

---

## 11. Known issues and lessons learned

### 11.1 Sky seam

- B01 không seamless: mép phải vàng sáng, mép trái xanh/tối.
- Dùng nhiều tile gây đường nối rõ sau khi chạy.
- Quyết định cuối: một sky sprite tĩnh phủ màn hình.

### 11.2 Wheel/body sorting

- Bánh từng bị body che do wheel order 3 và body order 4.
- Quyết định cuối: body 3, wheels 4, dust 5.
- Vị trí wheel/dust ban đầu cũng sai và đã được chỉnh thủ công; phải bảo toàn giá trị trong mục 7.

### 11.3 B06 canopy

- Transform cũ làm mất phần tre phía trên.
- Đã sửa để cover cả trên và dưới.

### 11.4 RuralLife omission

- Asset có trong repo không có nghĩa đã được đặt vào scene/prefab.
- Khi review task sau phải đối chiếu LDD acceptance checklist, không chỉ nhìn danh sách file asset.

### 11.5 Unity-generated whitespace

- `git diff --check` có thể báo trailing whitespace trong `.meta`, `.prefab` và `.unity` do Unity sinh dòng giá trị rỗng.
- Đây không phải lỗi runtime và không phải điều kiện bắt buộc để commit nếu project không có hook chặn.
- Không lãng phí thời gian chỉnh toàn bộ Unity YAML chỉ để xóa warning này.

### 11.6 Unity lock

- Nếu project đang mở/locked, Codex không được chạy thêm Unity Editor process cạnh tranh.
- Codex hoàn thành source rồi báo manual Unity steps.
- Người dùng chạy builder trong Unity instance đang mở.

---

## 12. Immediate next steps

### Step 1 — Finish RuralLife fix

Thực hiện checklist mục 8.3, sau đó merge `fix/m1a-rural-life` vào `main`.

### Step 2 — Synchronize local main

Sau khi merge:

```powershell
git switch main
git pull origin main
git status --short
```

Kỳ vọng `git status --short` không có output.

### Step 3 — Create a new narrow branch for Start Journey

Tên gợi ý:

```text
feat/m1a-start-journey-transition
```

Scope lượt đầu nên chỉ gồm:

1. chống double-click;
2. khóa menu input;
3. fade menu UI;
4. giữ truck/parallax tiếp tục chạy;
5. tạo hook/state để chuyển từ loop sang route;
6. chưa dựng toàn bộ B2 arrival trong cùng lượt.

### Step 4 — Later split route/arrival into separate tasks

- Route countryside/forest.
- Camp gate and yard.
- Truck stop.
- Disembark.
- Player handoff.
- Save/checkpoint.

Không gom tất cả vào một prompt lớn khi Codex usage còn hạn chế.

---

## 13. Master production checklist

### Main Menu

- [x] Main Menu foundation.
- [x] Visual asset pack/tank fallback.
- [x] First-time truck presentation.
- [x] Parallax environment.
- [x] Static seamless-safe sky approach.
- [x] Wheel/body/dust visual correction.
- [x] B06 canopy correction.
- [x] RuralLife visible in Unity.
- [ ] RuralLife committed/pushed/merged.
- [ ] Final 16:9 test after RuralLife merge.
- [ ] 16:10 test.
- [ ] 21:9 test.
- [ ] Final menu audio ambience.

### Start Journey / Opening Route

- [ ] Disable repeated clicks.
- [ ] Lock menu input.
- [ ] Fade UI.
- [ ] Preserve moving truck presentation.
- [ ] Convert loop state to authored route.
- [ ] Village segment.
- [ ] Rice-field segment.
- [ ] Forest transition.
- [ ] Gate segment.
- [ ] Yard segment.
- [ ] Stop-pad segment.
- [ ] Dialogue/radio timing.
- [ ] Copyright-safe decision for any historical song usage.

### Arrival / Player handoff

- [ ] Build B2 presentation.
- [ ] Truck stop animation/state.
- [ ] Lâm descend frames.
- [ ] Recruits disembark.
- [ ] Officer/instructor setup.
- [ ] Player disabled before grounded state.
- [ ] Correct Rigidbody2D/Collider2D/ground layer.
- [ ] Enable gameplay input exactly once.
- [ ] Camera follows player.
- [ ] Objective appears.
- [ ] Save opening completion/checkpoint.

### Màn 1 remainder

- [ ] Muster/dialogue.
- [ ] Movement tutorial.
- [ ] Obstacle/vault tutorial.
- [ ] AK training.
- [ ] Cover/trench.
- [ ] Grenade training.
- [ ] Fixed MG training.
- [ ] B41 training.
- [ ] Meet crew 314.
- [ ] Tank movement.
- [ ] Coax MG.
- [ ] Main gun/reload.
- [ ] Combined evaluation.
- [ ] End-of-mission state.
- [ ] Letter/diary loading screen.
- [ ] Switch menu state to tank 314.

### Foundation later

- [ ] Campaign save manager.
- [ ] Continue/checkpoints.
- [ ] New Journey reset confirmation.
- [ ] Localization/String Tables.
- [ ] Journal integration.
- [ ] Objective/mission system.
- [ ] Audio system for engine/wind/rural ambience/radio.

---

## 14. Handoff instructions for a new chat or Codex session

Khi bắt đầu chat/session mới, cung cấp file này cùng hai design docs và nói rõ:

1. Chỉ review/plan/prompt nếu đang nói với ChatGPT planning assistant.
2. Chỉ code khi prompt được người dùng tự dán vào Codex trong VS Code.
3. Trước mọi thay đổi phải chạy/đọc:
   - `git branch --show-current`
   - `git status --short`
   - current diff
   - `AGENTS.md`
   - file source liên quan.
4. Không restart/rewrite implementation đang có.
5. Không sửa protected scope.
6. Không commit hoặc push nếu prompt không cho phép.
7. Cuối task phải báo:
   - mọi file tạo/sửa;
   - việc đã hoàn thành/còn lại;
   - compile/builder/test result;
   - manual Unity steps;
   - blocker/error.

### Minimal handoff statement

```text
Read 314_Project_Progress_and_Checklist.md first. Treat its DONE / IN PROGRESS /
NOT STARTED status as authoritative for implementation progress, and use
314_LDD_01_Ngay_Nhan_Xe.md plus 314_M1A_Implementation_Brief.md for design.
Inspect the current Git branch/status/diff before proposing or changing anything.
Do not assume imported assets are already wired into runtime.
```

---

## 15. Current project status in one sentence

**Main Menu truck loop is implemented and merged, the missing RuralLife layer is visually working on a follow-up branch but still requires final idempotence testing and merge; Start Journey transition, authored route, B2 arrival, player handoff, save system, localization and the remaining Màn 1 gameplay have not yet been implemented.**
