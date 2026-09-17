# 314 — Gói B: Cảnh xe cam nhông v1

Bộ asset dùng cho đoạn mở đầu Màn 1: xe cam nhông chở tân binh đi qua đồng lúa, chuyển dần vào rừng và tới thao trường Đồng Lau.

## Phạm vi

- Camera 2D side-scrolling, góc nhìn ngang trực giao.
- Khung hình tham chiếu: `1920 × 1080`, tỉ lệ `16:9`.
- Phong cách: 2D vẽ tay, hiện thực cách điệu, cùng art direction với Gói A — Main Menu.
- Toàn bộ sprite cần tách nền đều là PNG RGBA có alpha thật.
- Không bake UI, hội thoại, số hiệu, cờ hoặc tên đơn vị vào hình.
- Biển thao trường để trống; chữ được gắn bằng TextMeshPro trong Unity.
- Âm thanh radio và nhạc không nằm trong gói này.

## Cấu trúc gói

| Thư mục | Nội dung |
|---|---|
| `Preview/` | Visual target và hai bản ghép kiểm tra 1920×1080 |
| `Background/` | Bầu trời, núi xa, làng, đồng lúa và lớp chuyển vào rừng |
| `Ground/` | Road texture để cuộn/lặp bằng overlap |
| `Foreground/` | Cỏ, tre, hàng rào và lá tiền cảnh |
| `Vehicle/` | Thân xe không bánh, hai bánh, tân binh và tài xế |
| `Characters/` | Ba nông dân và một trâu; có sheet tham chiếu và PNG riêng |
| `Props/` | Rơm, hàng rào, giỏ, xe kéo, chuối, cột mốc và biển trống |
| `VFX/` | Dust sheet chuẩn 4×2, 512×512 mỗi ô, cùng 8 frame riêng |
| `Docs/` | Hướng dẫn Unity và prompt tái tạo |

## File chính

- `B00_visual_target_1920x1080.png`: định hướng bố cục ban đầu.
- `B17_assembled_preview_1920x1080.png`: ghép trực tiếp từ asset production ở đoạn đồng lúa.
- `B18_forest_transition_preview_1920x1080.png`: ghép trực tiếp ở đoạn chuyển vào rừng.
- `B08_truck_body_no_wheels.png`: thân xe với cửa sổ/hốc bánh trong suốt.
- `B09_wheel_front.png`, `B10_wheel_rear.png`: bánh độc lập để xoay bằng Transform.
- `B11_recruits_row.png`, `B12_driver.png`: đặt phía sau thân xe trong hierarchy.
- `B15_dust_sheet_4x2.png`: sheet 2048×1024, chia đều `4 × 2`.

## Lưu ý production

- Các layer cảnh dài được thiết kế để lặp bằng hai bản sao có vùng overlap; chúng không phải texture seamless tuyệt đối từng pixel.
- Để giảm lộ nhịp lặp, có thể mirror bản sao thứ hai theo trục X và đổi nhẹ vị trí props.
- Preview chỉ là hướng dẫn lắp ráp, không dùng như một background đã bake.
- Nhạc `Đoàn Vệ quốc quân` không được cung cấp. Cần kiểm tra quyền sử dụng tác phẩm và bản thu trước khi phát hành.
- Asset là nền tảng cho vertical slice; trước bản thương mại nên có vòng art review cuối về lịch sử, phục trang và tối ưu texture.

Xem `Docs/UNITY_SETUP.md` để import và dựng scene.
