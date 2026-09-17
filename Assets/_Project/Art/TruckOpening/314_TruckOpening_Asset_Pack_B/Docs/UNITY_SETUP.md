# Unity Setup — Gói B: Cảnh xe cam nhông

## 1. Import

Copy toàn bộ thư mục gói vào:

```text
Assets/_Project/Art/TruckOpening/
```

Thiết lập chung cho PNG:

| Thuộc tính | Giá trị đề xuất |
|---|---|
| Texture Type | Sprite (2D and UI) |
| Pixels Per Unit | 100 |
| Filter Mode | Bilinear |
| Generate Mip Maps | Off |
| Alpha Is Transparency | On với file RGBA |
| Max Size — background/vehicle | 4096 |
| Max Size — props/VFX | 2048 |
| Compression | None khi dựng; High Quality khi tối ưu build |

Với background/parallax dùng `Mesh Type: Full Rect`. Với xe, người và props dùng `Mesh Type: Tight`.

`B15_dust_sheet_4x2.png`:

- Sprite Mode: Multiple
- Grid by Cell Size: `512 × 512`
- 4 cột × 2 hàng
- Pivot: Bottom Center

Các frame dust riêng trong `VFX/Dust/` đã được cắt sẵn, nên có thể bỏ qua slicing sheet.

## 2. Camera tham chiếu

Với Game View 1920×1080 và PPU 100:

```text
Projection: Orthographic
Orthographic Size: 5.4
Aspect: 16:9
```

## 3. Sorting Layers

| Order | Sorting Layer | Asset |
|---:|---|---|
| 0 | BG_Sky | `B01_sky_dawn_1920x1080` |
| 10 | BG_Mountains | `B02_mountains_far` |
| 20 | BG_Village | `B03_village_mid` |
| 30 | BG_FieldOrForest | `B04_rice_field_near` hoặc `B07_forest_transition` |
| 35 | BG_Ambient | Nông dân, trâu và props |
| 40 | Ground | `B05_road_dirt_loop` |
| 45 | VFX_Behind | Dust |
| 50 | Vehicle_Inside | Tân binh và tài xế |
| 55 | Vehicle_Wheels | Hai bánh xe |
| 60 | Vehicle_Body | Thân xe |
| 70 | FG_Foliage | `B06_foreground_bamboo_grass` |
| 100 | UI | Subtitle, tiêu đề màn, prompt |

## 4. Hierarchy xe

```text
TruckRig
├── BodyBob
│   ├── Recruits
│   ├── Driver
│   └── TruckBody
├── RearWheel
├── FrontWheel
└── DustEmitter
```

Giá trị khởi đầu nếu tất cả sprite dùng PPU 100 và pivot ở giữa:

| Object | Local Scale | Local Position gần đúng |
|---|---:|---:|
| TruckBody | 0.65 | (0, 0, 0) |
| RearWheel | 0.20 | (-3.0, -1.2, 0) |
| FrontWheel | 0.21 | (3.7, -1.2, 0) |
| Recruits | 0.31 | (-1.9, 0.45, 0) |
| Driver | 0.14 | (2.5, 0.15, 0) |

Đây là mốc lắp nhanh; tinh chỉnh vài pixel theo pivot thực tế trong Sprite Editor.

## 5. Chuyển động

Xe giữ gần giữa camera; môi trường trôi sang trái.

| Layer | Hệ số tốc độ gợi ý |
|---|---:|
| Sky | 0.00–0.03 |
| Mountains | 0.08 |
| Village | 0.18 |
| Rice field / forest | 0.35 |
| Road | 1.00 |
| Foreground | 1.15–1.25 |

- Tạo hai instance cho mỗi layer cuộn và dịch instance đã rời màn sang phía trước.
- Với road/village/field, overlap khoảng 96–160 px để che đường nối.
- Bánh xe quay clockwise khi cảnh trôi sang trái. Có thể tính `degreesPerSecond = speed / radius * Mathf.Rad2Deg` rồi dùng góc Z âm.
- `BodyBob` chỉ rung nhẹ: biên độ `0.02–0.04 unit`, tần số khoảng `2–3 Hz`.
- Dust chạy 8 frame ở `10–12 fps`, xuất hiện ngắt quãng sau bánh sau.

## 6. Chuyển đồng lúa sang rừng

1. Giảm dần alpha của `B03_village_mid` và `B04_rice_field_near`.
2. Đưa `B07_forest_transition` từ phải sang trái với cùng tốc độ middle-ground.
3. Giảm âm lượng radio, tăng tiếng ve, bánh trên đường đất và tiếng súng thao trường từ xa.
4. Đặt `B16_training_sign_blank` ở middle-ground.
5. Gắn TextMeshPro World Space lên mặt biển:

```text
THAO TRƯỜNG ĐỒNG LAU
KHU VỰC HUẤN LUYỆN
```

Không sửa trực tiếp chữ vào PNG để giữ khả năng đa ngôn ngữ.

## 7. UI và thời lượng

- Cảnh tự chạy đề xuất: 30–45 giây.
- Subtitle nằm trong Safe Area, không gắn vào background.
- Tiêu đề màn xuất hiện khi xe qua biển thao trường.
- Sau khi xe dừng, tách sang animation xuống xe hoặc chuyển scene gameplay; gói này chưa bao gồm animation xuống xe.
