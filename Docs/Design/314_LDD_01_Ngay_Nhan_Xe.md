# 314 — LEVEL DESIGN DOCUMENT 01

## Màn 1: Ngày nhận xe

**Tên file:** `314_LDD_01_Ngay_Nhan_Xe.md`  
**Dự án:** `314`  
**Engine:** Unity 6 — game 2D nhìn ngang  
**Phiên bản tài liệu:** 0.1 — Consolidated Source of Truth  
**Ngày tổng hợp:** 2026-09-17  
**Tài liệu nền:** `314_Game_Design_Bible_Phase_1.md` v5 và toàn bộ trao đổi thiết kế Màn 1  
**Trạng thái:** Tiền sản xuất / chốt cốt truyện, cơ chế và yêu cầu prototype  

---

## 0. Mục đích của tài liệu

Tài liệu này tập hợp **toàn bộ nội dung đã trao đổi liên quan trực tiếp đến Màn 1**, để có thể mang sang một conversation hoặc nhánh phát triển khác mà không phụ thuộc vào trí nhớ của luồng chat cũ.

Đây là tài liệu nguồn chính cho:

- Cốt truyện và nhịp cảm xúc Màn 1.
- Luồng từ Main Menu sang gameplay.
- Tutorial bộ binh và tutorial xe tăng.
- Điều khiển, đạn dược, thay đạn thủ công và HUD.
- Hội thoại bằng text.
- Checkpoint, save, fail/retry và kết thúc màn.
- Background, bố cục 2D và asset cần dùng.
- Loading screen, nhật ký và vật lưu niệm.
- Localization tiếng Việt trước, mở rộng tiếng Anh sau.
- Những quyết định đã bị thay thế, còn xung đột hoặc chưa khóa.

### Quy ước trạng thái

| Nhãn | Ý nghĩa |
|---|---|
| **LOCKED** | Đã chốt, chỉ đổi khi có quyết định mới rõ ràng từ chủ dự án. |
| **PROTOTYPE** | Giá trị mặc định để làm thử và cân bằng, có thể chỉnh sau playtest. |
| **OPEN** | Chưa chốt; phải quyết định trước khi sản xuất phần liên quan. |
| **REPLACED** | Ý tưởng từng được nêu nhưng đã có phương án mới thay thế. |
| **CUT** | Không sử dụng trong thiết kế hiện tại. |

Nếu một nội dung vừa có phương án cũ vừa có phương án mới, tài liệu giữ lại cả hai và đánh dấu rõ, không âm thầm xóa phương án cũ.

---

# 1. Trụ cột dự án áp dụng cho Màn 1

## 1.1. Bản sắc

- **LOCKED:** Game 2D nhìn ngang, di chuyển chủ yếu từ trái sang phải.
- **LOCKED:** Game xoay quanh hành trình của xe tăng số hiệu **314** và những con người gắn với nó.
- **LOCKED:** Mọi nhân vật, đơn vị, địa danh và trận đánh trực tiếp trong game đều hư cấu.
- **LOCKED:** Bối cảnh chỉ lấy cảm hứng từ Việt Nam giai đoạn đầu–giữa thập niên 1970; không ghi một mốc năm chính xác trong Màn 1.
- **LOCKED:** Không làm cutscene điện ảnh tách khỏi gameplay.
- **LOCKED:** Không có voice-over; toàn bộ hội thoại và radio được thể hiện bằng text/subtitle.
- **LOCKED:** Có thể dùng scripted sequence trong engine khi người chơi chưa điều khiển, miễn camera và hình ảnh vẫn thuộc cùng không gian 2D của game.
- **LOCKED:** Tiếng Việt là ngôn ngữ gốc. Nội dung phải được thiết kế từ đầu để thêm tiếng Anh và các ngôn ngữ khác về sau.

## 1.2. Chủ đề

Màn 1 phải thiết lập ngay các chủ đề xuyên suốt:

- Tuổi đôi mươi, lý tưởng, tinh thần giải phóng đất nước và mong muốn hòa bình.
- Những người trẻ xa lạ bắt đầu trở thành một kíp xe.
- Xe tăng mạnh nhưng không bất tử và không thể chiến đấu tách rời bộ binh.
- Bắn trúng chưa chắc đã là bắn đúng lúc.
- Kỷ luật, nhận thức chiến trường và trách nhiệm với người đi cạnh xe quan trọng hơn thành tích cá nhân.
- Chiến tranh không được kể như một cuộc thi đếm số kẻ địch bị tiêu diệt.

## 1.3. Vai trò của Màn 1 trong campaign

Màn 1 là chương mở đầu và tutorial tổng hợp. Người chơi cần kết thúc màn với ba cảm giác:

1. Đã hiểu đủ điều khiển bộ binh và xe tăng để bước vào Màn 2.
2. Đã biết năm nhân vật chính và mối quan hệ ban đầu giữa họ.
3. Đã có sự gắn bó đầu tiên với xe 314, nhưng hiểu rằng chiếc xe chỉ hoạt động được nhờ cả kíp và bộ binh hỗ trợ.

---

# 2. Thông tin tổng quan Màn 1

| Hạng mục | Thiết kế |
|---|---|
| Tên màn | **Ngày nhận xe** |
| Địa danh | **Thao trường Đồng Lau** — hư cấu |
| Thời lượng lần đầu | **18–20 phút** — LOCKED về mục tiêu, cần playtest để cân bằng |
| Thời điểm | Ban ngày, ánh nắng dịu; không mưa |
| Bối cảnh mở đầu | Đường quê, đồng lúa chín, nông dân, trâu, khu dân cư, bìa rừng |
| Bối cảnh chính | Doanh trại, các khu thao trường bộ binh, bãi xe và thao trường xe tăng |
| Nhịp đầu | Thanh bình, trẻ trung, có tiếng cười và âm nhạc radio |
| Nhịp giữa | Học tập, làm quen, hơi hài hước nhưng có kỷ luật |
| Nhịp cuối | Bài sát hạch căng vừa đủ, kết thúc bằng bài học trách nhiệm |
| Kẻ địch thật | Không có |
| Mini-boss | Không có; cao trào là bài sát hạch phối hợp |
| Cutscene | Không có cutscene tách biệt; dùng scripted sequence 2D |
| Phần thưởng | Ảnh kíp xe 314, trang nhật ký của Quang, mở Màn 2 |
| Trạng thái menu sau màn | Xe cam nhông được thay bằng xe 314 tại doanh trại |

---

# 3. Nhân vật xuất hiện

## 3.1. Nhân vật chính

| Nhân vật | Tuổi định hướng | Vai trò | Ấn tượng cần tạo ở Màn 1 |
|---|---:|---|---|
| **Lâm** | 20 | Pháo thủ, nhân vật người chơi | Từng là sinh viên kỹ thuật; nhanh nhạy, muốn chứng tỏ mình, có phần nóng lòng khai hỏa. |
| **Khánh** | 25 | Trưởng xe 314 | Điềm tĩnh, ít nói, đặt an toàn và kỷ luật lên trên thành tích. |
| **Hoàng** | 23 | Lái xe kiêm người hiểu máy móc nhất kíp | Thực tế, chăm xe, nói chuyện với chiếc xe như với một thành viên. |
| **Quang** | 19 | Nạp đạn viên, người ghi nhật ký | Từng là sinh viên thiên về văn chương/giáo dục; cởi mở, quan sát tinh tế, tạo chất đời thường. |
| **Sơn** | 22 | Tổ trưởng bộ binh | Từng là sinh viên lâm nghiệp; thận trọng, hiểu địa hình và vai trò của bộ binh quanh xe tăng. |

Các tuổi và xuất thân trên là định hướng đã được chọn theo tinh thần “những người tuổi đôi mươi hoặc sinh viên tập kết”. Có thể tinh chỉnh tiểu sử sau, nhưng không được làm mất tinh thần này.

## 3.2. Nhân vật phụ không cần đặt tên

- Sĩ quan/cán bộ tiếp nhận tân binh.
- Cán bộ thao trường hoặc hướng dẫn viên khu di chuyển.
- Cán bộ cấp phát trang bị.
- Các tân binh đi cùng xe cam nhông.
- Một số bộ binh tham gia bài sát hạch cuối.
- Nông dân và người dân trong background mở đầu.

**LOCKED:** Không cần đặt tên cho từng thành viên phụ trong tổ của Sơn ở giai đoạn này.

## 3.3. Tổ bộ binh của Sơn

Tổ của Sơn là lực lượng sẽ phối hợp cùng xe 314 trong campaign. Màn 1 chỉ giới thiệu họ qua thao trường và nhóm bộ binh đi qua vùng bắn ở bài sát hạch; không cần giới thiệu từng người.

Silhouette/biên chế định hướng:

- Sơn — tổ trưởng, dùng AK.
- Lính súng trường AK.
- Xạ thủ RPD.
- Lính chống tăng B41.
- Có thể thêm một lính mang đạn/liên lạc khi scope cho phép.

Ở các màn chiếm cứ điểm sau này, tổ Sơn có thể kéo cờ sau khi khu vực an toàn. Đây không phải thao tác gameplay của Màn 1.

## 3.4. Cấu trúc kể chuyện

- **LOCKED:** Cốt truyện Màn 1 tuyến tính.
- Không có lựa chọn hội thoại tạo nhánh hoặc ending khác.
- Các tương tác quanh xe 314 có thể cho người chơi chọn thứ tự đọc/quan sát, nhưng cuối cùng vẫn hội tụ vào cùng bài huấn luyện.
- Nếu thêm lựa chọn text, chúng chỉ thay đổi một câu phản hồi hoặc sắc thái quan hệ; không tạo nhánh sản xuất mới.

---

# 4. Mạch cảm xúc

| Đoạn | Cảm xúc chủ đạo | Điều người chơi học về câu chuyện |
|---|---|---|
| Xe cam nhông trên đường quê | Yên bình, trẻ trung, háo hức | Những người trẻ đang rời đời sống bình thường để tới đơn vị. |
| Trình diện | Kỷ luật nhưng không lạnh lùng | Từ cá nhân trở thành một phần của tập thể. |
| Huấn luyện bộ binh | Tò mò, thử thách, có tiếng cười | Lâm phải biết tự bảo vệ mình trước khi ngồi trong xe. |
| Gặp Sơn | Tôn trọng bộ binh | Người đi bên cạnh xe không có lớp thép bảo vệ. |
| Gặp xe 314 | Kinh ngạc, gắn bó bắt đầu | Chiếc xe là nơi bốn người sẽ cùng sống và chiến đấu. |
| Huấn luyện xe | Sức mạnh đi cùng quy trình | Mỗi thao tác phụ thuộc vào thành viên khác. |
| Sát hạch tổng hợp | Căng thẳng, kiềm chế | Người pháo thủ phải nhìn cả chiến trường, không chỉ tâm ngắm. |
| Ảnh kíp xe | Ấm áp, dự cảm hành trình | Bốn người chính thức trở thành một kíp xe. |

---

# 5. Luồng tổng thể A–K

| Mục | Tên đoạn | Nội dung chính |
|---|---|---|
| A | Menu xe cam nhông | Menu đầu game là xe cam nhông chạy vô hạn trên đường quê. |
| B | Đường tới thao trường | Nhấn Bắt đầu hành trình; UI mờ dần, xe tiếp tục đi qua đồng lúa, khu dân cư, bìa rừng và cổng doanh trại. |
| C | Xuống xe và trình diện | Lâm cùng tân binh xuống xe, xếp hàng, nghe sĩ quan phổ biến. |
| D | Huấn luyện di chuyển | Học đi, chạy, cúi, vượt vật cản và tương tác; đi qua cổng nội bộ “THAO TRƯỜNG”. |
| E | Huấn luyện AK | Học ngắm, bắn, semi/auto, điểm xạ và thay đạn thủ công. |
| F | Cover, chiến hào và lựu đạn | Học dùng địa hình, vật chắn, xuống hào và ném lựu đạn. |
| G | Trạm súng máy/B41 | Hai trường bắn riêng cho súng máy cố định và B41. |
| H | Gặp kíp xe 314 | Lâm và Quang được gọi sang bãi cơ giới; gặp Khánh, Hoàng và chiếc xe 314. |
| I | Huấn luyện lái và bắn pháo | Học tiến/lùi, dừng, nâng/hạ nòng, đồng trục, pháo chính và lệnh nạp thủ công. |
| J | Bài sát hạch tổng hợp | Phối hợp di chuyển, hỏa lực và chờ bộ binh qua vùng nguy hiểm. |
| K | Kết màn và loading screen | Về bãi, chụp ảnh kíp xe, ghi nhật ký, autosave, mở Màn 2. |

---

# 6. Timeline mục tiêu 18–20 phút

Đây là ngân sách thời gian **PROTOTYPE**, không phải giới hạn cứng. Người chơi mới có thể lâu hơn một chút.

| Thời gian | Nội dung |
|---:|---|
| 00:00–00:35 | Menu xe cam nhông và chuyển từ menu sang hành trình |
| 00:35–02:15 | Đường tới doanh trại, qua cổng, dừng xe, xuống xe và trình diện |
| 02:15–03:30 | Huấn luyện di chuyển cơ bản |
| 03:30–06:45 | Trường bắn AK: semi, auto, điểm xạ, manual reload |
| 06:45–09:15 | Cover, chiến hào, vật cản và lựu đạn |
| 09:15–11:15 | Súng máy cố định và B41 |
| 11:15–13:15 | Chuyển sang bãi xe, gặp kíp 314 và tương tác quanh xe |
| 13:15–17:15 | Huấn luyện lái, đồng trục, AP/HE và nạp pháo |
| 17:15–19:30 | Bài sát hạch tổng hợp |
| 19:30–20:00 | Về bãi, ảnh kíp xe, nhật ký, save và loading screen |

Nếu cần rút gần 18 phút:

- Mỗi bài chỉ yêu cầu thành công một lần.
- Không bắt người chơi bắn nhiều lượt để đạt điểm cao.
- Giữ đoạn gặp kíp xe làm nhịp nghỉ, không cắt bỏ hoàn toàn.
- Trạm súng máy/B41 có thể rút số mục tiêu, nhưng không ghép chung một trường bắn.

---

# 7. Bố cục bản đồ 2D

## 7.1. Nguyên tắc

- **LOCKED:** Toàn bộ gameplay nhìn ngang.
- **LOCKED:** Di chuyển chính trái–phải; không có điều khiển chiều sâu.
- **CUT:** Không có bài lái xe ziczac kiểu top-down hoặc lái qua nhiều làn chiều sâu.
- **LOCKED:** Camera không chuyển sang góc nhìn sau lưng ở trường bắn.
- **LOCKED:** Mục tiêu được bố trí dọc trục ngang, chủ yếu về bên phải người chơi.
- Có thể dùng cổng, mô đất, hàng rào và đoạn camera chuyển ngắn để ngầm hiểu nhân vật đã sang một khu tập khác.

## 7.2. Chuỗi khu vực

1. Đường quê/menu loop.
2. Khu dân cư và đồng lúa.
3. Bìa rừng.
4. Cổng ngoài doanh trại.
5. Sân đón quân và khu trình diện.
6. Cổng gỗ nội bộ “THAO TRƯỜNG”.
7. Bãi di chuyển/vật cản cơ bản.
8. Trường bắn AK.
9. Bãi cover và chiến hào.
10. Bãi lựu đạn.
11. Trường súng máy cố định.
12. Trường B41.
13. Đường chuyển sang bãi cơ giới.
14. Bãi xe tăng.
15. Thao trường xe tăng.
16. Bãi đỗ kết màn.

Không bắt buộc tất cả khu vực tồn tại trên một mặt phẳng vật lý liên tục. Khi cần tiết kiệm chiều dài map hoặc memory, dùng loading nội bộ rất ngắn/fade/cổng chuyển khu nhưng vẫn giữ cảm giác hành trình từ trái sang phải.

## 7.3. Hai loại cổng không được nhầm

### Cổng ngoài doanh trại

- Thiết kế đã có trong asset B2.
- Biển đỏ cũ, sao vàng và dòng chữ:

  **DOANH TRẠI**  
  **QUÂN ĐỘI NHÂN DÂN VIỆT NAM**

- Nên dùng biển tách lớp hoặc TextMeshPro để hỗ trợ localization; bản chữ ghép sẵn chỉ dùng khi chấp nhận tiếng Việt cố định trong môi trường.

### Cổng nội bộ thao trường

- Cổng gỗ đơn giản.
- Dùng chữ **THAO TRƯỜNG**.
- Có thể tái sử dụng blank sign từ asset pack và đổ chữ bằng TextMeshPro.
- Dùng để báo người chơi đã đi từ khu tiếp nhận sang khu huấn luyện.

---

# 8. Thiết kế chi tiết từng đoạn

## A. Main Menu — xe cam nhông chạy vô hạn

### Mục đích

- Tạo ấn tượng đầu tiên về tuổi trẻ và quê hương thanh bình.
- Biến Main Menu thành phần mở đầu câu chuyện thay vì một màn hình tách biệt.
- Cho phép cảnh menu nối liền sang gameplay khi bắt đầu hành trình.

### Trạng thái menu lần đầu

- Không có `CampaignSave`: menu hiển thị xe cam nhông đang chạy trên đường quê.
- Background parallax lặp: đồng lúa, nông dân, trâu, mái nhà, núi xa, cây và bụi tiền cảnh.
- Xe cam nhông giữ chuyển động bánh, thân rung nhẹ, bụi đường và parallax; bản thân xe có thể gần như cố định trong khung hình để tạo vòng lặp vô hạn.
- Menu nằm phía trái hoặc vùng tối dễ đọc; xe nên lệch phải vừa đủ để không đè UI.
- Có thể dùng gradient tối nhẹ phía trái để tăng độ tương phản chữ. Đây là tinh chỉnh **PROTOTYPE**.
- Âm thanh: động cơ, bánh xe, gió và không khí nông thôn.

### Các trạng thái Main Menu theo campaign

| Tiến trình | Cảnh menu |
|---|---|
| Chưa bắt đầu | Xe cam nhông trên đường quê; dùng cho opening Màn 1. |
| Đã nhận xe 314 / hoàn thành Màn 1 | Xe 314 tại doanh trại. |
| Đi được khoảng nửa campaign | Xe 314 bám bùn, có vết hư hỏng và đồ dùng tích lũy. |
| Hoàn thành game | Xe 314 đứng yên trong ánh sáng buổi sáng, nhấn mạnh hòa bình và sự im lặng sau hành trình. |

Các trạng thái này phục vụ chủ đề “tuổi đôi mươi, lý tưởng, giải phóng đất nước và yêu hòa bình”; không dùng menu chỉ để phô diễn chiến tích.

### Khi chọn “Bắt đầu hành trình”

1. Khóa input menu.
2. Các nút menu mờ dần.
3. Camera và xe không cắt sang một shot khác; vòng lặp được chuyển thành một tuyến đường có điểm đến.
4. Xe tiếp tục chạy qua khu dân cư, đồng lúa, bìa rừng, cổng doanh trại và dừng ở sân đón quân.
5. Khi Lâm chạm đất và quyền điều khiển được trao cho người chơi, ghi `opening_truck_completed = true`.

### Trường hợp đã có save

- Continue tải checkpoint gần nhất; không phát lại đoạn xe cam nhông.
- Từ Màn 2 trở đi, Main Menu hiển thị trạng thái campaign hiện tại, không tự quay lại truck loop.
- Sau khi hoàn thành Màn 1, menu mặc định đổi thành xe 314 trong doanh trại.

### Hành trình mới

- Khi đã có CampaignSave và chọn **Bắt đầu hành trình mới**, hiện modal cảnh báo tiến trình cũ sẽ bị xóa.
- Nếu hủy: trở lại menu hiện tại.
- Nếu đồng ý: xóa `CampaignSave`, giữ `GlobalSettings`, trở lại đúng cảnh truck menu ban đầu rồi tự bắt đầu hành trình; không bắt người chơi bấm Start lần thứ hai.

### Save giữa đoạn mở đầu

- Nếu thoát trước khi `opening_truck_completed`, lần sau phát lại toàn bộ opening.
- Nếu đã hoàn thành opening, Continue không phát lại.

---

## B. Đường tới thao trường

### Hình ảnh

- Xe đi trên đường lớn nhìn ngang.
- Hai bên là đồng lúa chín, nông dân làm việc, trâu, nhà dân và cảnh quê thanh bình.
- Dần rời khu dân cư, cây cối dày hơn, chuyển qua bìa rừng rồi tới cổng doanh trại.
- Không khí không u ám; đây là khoảnh khắc trước khi các nhân vật thực sự bước vào hành trình chiến tranh.

### Radio và âm nhạc

- Mong muốn hiện tại: radio trên xe phát một đoạn ngắn mang không khí hành quân, được nhắc tới là bài **“Đoàn Vệ quốc quân”**.
- Âm thanh nên nhỏ, mono/rè, nghe như phát từ chiếc radio trong cabin hoặc thùng xe.
- Radio nhỏ dần khi xe đi vào khu rừng/doanh trại.
- Không phát toàn bài và không dùng lời dài trên màn hình.
- **OPEN:** Phải kiểm tra quyền sử dụng bản nhạc, bản ghi và lời ca trước production. Nếu chưa rõ, dùng nhạc hành khúc nguyên bản do dự án tự làm với placeholder `RADIO_TRACK_MARCH_PLACEHOLDER`.
- **REPLACED:** Ý tưởng ban đầu dùng “Tiến quân ca” và hiển thị lời đã được thay bằng hướng “Đoàn Vệ quốc quân”/nhạc hành khúc radio. Không đưa lời bài hát dài vào tài liệu hay game khi chưa xử lý quyền sử dụng.

### Hội thoại/không khí trên xe

- Các tân binh nói chuyện và cười bằng text bubble/subtitle ngắn.
- Không cần giới thiệu toàn bộ nhân vật chính trong xe.
- Quang có thể xuất hiện trong nhóm nhưng phần giới thiệu trực tiếp diễn ra sau khi xuống xe.
- Tránh để đoạn này chỉ có âm nhạc; cần vài dòng text ngắn để người chơi cảm nhận đây là những người trẻ đang đi nhận đơn vị.

### Trigger asset B2

| Marker | Công dụng |
|---|---|
| `ArrivalGateEntered` | Xe đi qua cổng doanh trại; radio/ambience chuyển trạng thái. |
| `TruckBrakeStart` | Bắt đầu animation giảm tốc và bụi giảm. |
| `TruckStop` | Xe dừng đúng pad. |
| `LamGrounded` | Lâm chạm đất; mở input gameplay và ghi checkpoint đầu. |

Tổng chiều dài scripted route mục tiêu từ khi nhấn Start đến khi điều khiển được Lâm: khoảng **25–35 giây**.

---

## C. Khu 1 — Bãi đón quân, xuống xe và trình diện

### Script vào khu

- Xe dừng nhưng camera vẫn là camera gameplay ngang.
- Các tân binh lần lượt xuống trước.
- Quang xuống ngay trước Lâm.
- Lâm dùng chuỗi animation xuống xe B2; khi hai chân chạm đất mới trao điều khiển.
- Không dùng cinematic camera hoặc close-up khuôn mặt.

### Hội thoại giới thiệu Lâm–Quang

> **Quang:** “Cậu cũng về đơn vị tăng à?”  
> **Lâm:** “Ừ. Pháo thủ.”  
> **Quang:** “Tôi là Quang, nạp đạn. Vậy chắc còn gặp nhau dài.”

### Gameplay nhẹ

- Đi trái/phải.
- Chạy theo nhóm.
- Tương tác với Quang.
- Tới điểm tập trung.
- Xếp hàng/trình diện.
- Nhận trang bị.

### Sĩ quan tiếp nhận

Sĩ quan nói vài câu ngắn trước khi tách nhóm ra thao trường. Nội dung cần truyền đạt:

- Chào mừng/tiếp nhận quân.
- Trong doanh trại và thao trường phải tuân lệnh.
- Chỉ dùng vũ khí khi có lệnh.
- Tổ của Lâm theo cán bộ hướng dẫn sang khu huấn luyện.

**OPEN:** Lời thoại cuối cùng của sĩ quan chưa khóa. Bản nháp production có thể là:

> **Sĩ quan:** “Từ hôm nay, các đồng chí thuộc biên chế huấn luyện Đồng Lau.”  
> **Sĩ quan:** “Trên thao trường, mệnh lệnh và an toàn đi trước thành tích.”  
> **Sĩ quan:** “Theo hướng dẫn viên. Chỉ sử dụng vũ khí khi có lệnh.”

### Bàn cấp phát

> **Cán bộ thao trường:** “AK, một băng trên súng, ba băng dự trữ. Hai quả lựu đạn tập.”  
> **Cán bộ thao trường:** “Chỉ được lên đạn khi có lệnh.”

HUD lần đầu xuất hiện:

```text
AK: 30 / 90
Lựu đạn: 2
```

Giải thích:

- 30 viên trong băng gắn trên súng.
- 90 viên dự trữ, tương đương 3 băng.
- 2 quả lựu đạn tập.
- **LOCKED:** Không thể khai hỏa trong khu đón quân.
- Nếu người chơi bấm bắn, hiển thị nhắc ngắn: `CHƯA ĐƯỢC PHÉP KHAI HỎA`.

### Checkpoint

`CP01_Arrival_LamGrounded`

- Ghi sau `LamGrounded` và khi player control đã bật.
- Đồng thời ghi `opening_truck_completed = true`.
- Đây là checkpoint đầu tiên và là ranh giới để Continue không phát lại opening.

---

## D. Khu huấn luyện di chuyển

### Chuyển khu

- Nhóm đi qua cổng gỗ nội bộ có chữ **THAO TRƯỜNG**.
- Hướng dẫn viên đứng bên cạnh để hướng dẫn từng thao tác.
- Vũ khí vẫn bị khóa cho tới trường bắn AK.

### Kỹ năng được dạy

1. `A / D`: đi trái/phải.
2. `Shift`: chạy khi đi bộ.
3. `Ctrl`: cúi/núp thấp.
4. `Space` gần vật cản thấp: vượt vật cản.
5. `E`: tương tác với cổng, người hướng dẫn hoặc điểm quy định.

### Bố trí

- Một đoạn đất phẳng ngắn.
- Cọc/mốc chỉ hướng.
- Một vật cản thấp để vault.
- Một bao cát để thử cúi.
- Cổng hoặc bảng chuyển sang trường bắn AK.

### Quy tắc

- Không cần điểm số.
- Mỗi prompt biến mất ngay khi người chơi thực hiện đúng.
- Không dùng “đường ziczac”; đây là game 2D đi ngang.
- Hướng dẫn không kéo dài quá 60–75 giây.

---

## E. Khu 2 — Trường bắn AK

### Gặp Sơn

Lâm và Quang đến trường bắn, gặp Sơn lần đầu.

> **Sơn:** “Tôi là Sơn, phụ trách tổ bộ binh huấn luyện hôm nay.”  
> **Sơn:** “Xe tăng có lớp thép. Người đi bên cạnh xe thì không. Trước khi học lái tăng, các cậu phải biết tự bảo vệ mình.”

Đây là câu chủ đề đầu tiên về quan hệ xe tăng–bộ binh.

### Bắt đầu bài bắn

Tại vạch bắn:

```text
[E] Báo cáo sẵn sàng
```

Sau khi tương tác:

> **Sơn:** “Lắp băng.”  
> **Sơn:** “Chuột phải để ngắm. Khi chắc mục tiêu, chuột trái để bắn.”

### Điều khiển AK

- `Chuột phải`: bật/tắt ngắm; nhấn một lần bật, nhấn lần nữa tắt.
- `Chuột trái`: bắn.
- `R`: thay băng thủ công.
- `B`: chuyển `SEMI` / `AUTO`.
- Không có chế độ burst riêng.
- Không tự thay băng khi hết đạn.
- Bị khóa bắn trong khi nạp đạn hoặc tương tác.

### Bia và góc nhìn

- Bia nằm hoàn toàn trên trục ngang, về phía bên phải.
- Dùng bia cơ khí bật lên/hạ xuống, silhouette nhìn ngang.
- Không dùng bia giấy quay mặt trực diện camera.
- Không bắn vào chiều sâu.
- Camera không chuyển sang góc nhìn sau lưng.

### Ba bài ngắn

| Bài | Bố trí | Kỹ năng |
|---|---|---|
| Bia gần | Lâm bên trái, 3 bia bên phải | RMB ngắm, LMB bắn, SEMI |
| Bia có che chắn | Bia bật lên sau bao cát/mô đất | Cúi, chọn thời điểm, không xả đạn |
| Bia xa/trên cao | Bia trên ụ đất hoặc tháp thấp | Điều chỉnh hướng ngắm bằng chuột |

### Manual reload được giới thiệu trong tình huống

Sau loạt đầu:

> **Sơn:** “Dừng bắn. Kiểm tra súng.”  
> **Sơn:** “Không có ai thay băng hộ cậu đâu. Nhấn R.”

Nếu băng hết mà người chơi bấm bắn:

```text
BĂNG ĐẠN TRỐNG
Nhấn R để thay băng
```

### Bài điểm xạ ở AUTO

Hội thoại:

> **Sơn:** “Chuyển sang liên thanh.”  
> **Lâm:** “Giữ cò đến khi bia đổ à?”  
> **Sơn:** “Không. Mỗi loạt hai viên.”  
> **Sơn:** “Bóp vào, thả ngay, da tay dính cò.”  
> **Quang:** “Nghe thì dễ.”  
> **Sơn:** “Ở thao trường, cái gì cũng dễ.”

Objective:

```text
Bắn trúng 3 bia bằng điểm xạ ngắn
```

Đánh giá mỗi loạt:

| Số viên trong một lần giữ cò | Kết quả |
|---:|---|
| 2 viên | **Tốt** |
| 3 viên | **Đạt** |
| 4 viên trở lên | **Chưa đạt** |

Quy tắc:

- Không ép chính xác tuyệt đối 2 viên trong mọi tình huống.
- 2–3 viên được tính hoàn thành.
- Giữ cò lâu làm tâm ngắm nở nhanh, nòng súng bị đẩy lên và những viên sau tản rộng.
- Sơn chỉ nhắc một lần: **“Thả cò!”**
- Không trừ điểm nặng hoặc bắt chơi lại ngay; cho người chơi thử bia tiếp theo.

Khi hoàn thành:

> **Sơn:** “Đúng rồi. Hai tiếng một. Nhớ lấy nhịp đó.”

### Motif cho các màn sau

Nhịp điểm xạ của Lâm có thể trở thành âm thanh nhận diện giữa đồng đội. Một màn sau, Sơn có thể nói:

> **Sơn:** “Nghe tiếng súng là biết cậu còn ở đó.”

Đây là foreshadowing, không phát ngay trong Màn 1.

### Xung đột câu khẩu quyết cần giữ lại

- Game Design Bible hiện ghi: **“Bóp vào nhả ngay, ra tay dính cò.”**
- Bản trao đổi sau ghi: **“Bóp vào, thả ngay, da tay dính cò.”**
- **OPEN:** Cần xác minh và chọn đúng một phiên bản trước khi khóa localization. Bản thứ hai đang được dùng tạm trong flow phía trên vì là phiên bản xuất hiện sau.

### Checkpoint

`CP02_AK_Completed`

- Ghi sau khi hoàn thành cả SEMI, manual reload và AUTO/điểm xạ.
- Một phương án cũ từng đặt checkpoint ngay sau tutorial di chuyển; phương án hiện tại gom tới sau AK để tránh checkpoint quá dày.

---

## F. Khu 3 — Cover, vật cản, chiến hào và lựu đạn

### Chuyển đoạn

Trước khi vào bãi vật cản, Lâm hạ súng khỏi tư thế ngắm. Sơn đi trước và minh họa bằng animation đơn giản.

> **Sơn:** “Ngoài thao trường, ít khi cậu được đứng thẳng mà bắn.”  
> **Quang:** “Trong xe thì tôi còn chẳng đứng nổi.”  
> **Sơn:** “Vậy cậu càng nên tập.”

### Chuỗi gameplay

1. Chạy qua một đoạn đất trống.
2. Cúi sau bao cát.
3. Vượt tường thấp.
4. Đi xuống chiến hào theo lối dốc.
5. Ngắm và bắn bia từ trong hào.
6. Di chuyển trong hào.
7. Leo/đi ra bằng lối dốc cuối hào.

### Quy tắc cover

- `Ctrl` làm nhân vật cúi thấp.
- Vật chắn có collider thật và chặn đường đạn.
- Cúi không tự làm nhân vật bất tử.
- Không làm cơ chế bám tường phức tạp.
- Chiến hào là một phần địa hình thấp xuống, không có nút “vào chiến hào”.

### Bài lựu đạn

Khu lựu đạn được tách bởi tường chắn/mô đất.

> **Sơn:** “Chốt vẫn còn thì nó chưa làm hại ai.”  
> **Sơn:** “Giữ G để chọn lực. Thả ra để ném. Ném xong cúi xuống.”

Người chơi:

1. Đứng sau tường chắn.
2. Giữ `G` để chọn lực.
3. Nhìn đường dự báo quỹ đạo trong tutorial.
4. Thả `G` để ném vào hào mục tiêu/bia sau bao cát.
5. Cúi sau vật chắn.
6. Quan sát vụ nổ và phản hồi của Sơn.

Quy tắc:

- Quỹ đạo dự báo chỉ bắt buộc ở tutorial; có thể giảm hỗ trợ về sau.
- Dùng 1 quả để hoàn thành.
- Quả thứ hai dùng cho lần thử lại hoặc được giữ cho bài tổng hợp nếu thiết kế sau cần.
- **PROTOTYPE:** Nếu ném ra ngoài vùng an toàn, reset ngay quả tập và đặt lại trạng thái bài; không hard fail màn.

---

## G. Khu 4 — Bãi hỏa lực: súng máy cố định và B41

### Nguyên tắc an toàn và bố cục

- Súng máy và B41 là **hai trường bắn riêng**.
- Không đặt chung với trường AK trên cùng một lane gần nhau.
- Các khu được ngăn bằng mô đất cao, hàng rào, bảng phân khu, đường chuyển tiếp và bệ bắn riêng.
- Trên bản đồ 2D chúng có thể nối tiếp từ trái sang phải, nhưng phải tạo cảm giác đã sang khu khác.

## G1. Trạm súng máy cố định

> **Sơn:** “Súng này giữ được một hướng, nhưng cũng giữ chân người sử dụng nó.”  
> **Sơn:** “Nhấn E để vào vị trí. Muốn rời súng, nhấn E lần nữa.”

Người chơi:

1. Nhấn `E` để sử dụng súng máy.
2. Điều chỉnh hướng ngắm trong cung giới hạn.
3. Bắn nhóm bia cơ khí.
4. Khi phần đạn sẵn sàng hết, nhấn `R` để thay dây/hộp.
5. Nhấn `E` để rời vị trí.

Quy tắc:

- Người chơi không thể di chuyển khi đang vận hành súng cố định.
- Súng máy có “đạn sẵn sàng” và “đạn dự trữ” tách biệt.
- Dây 7,62 mm tham khảo khoảng 250 viên cho hệ thống chung; Màn 1 nên dùng lượng tập nhỏ hơn để bài không biến thành xả đạn kéo dài.
- **OPEN:** Số viên chính xác của trạm huấn luyện cần chốt khi prototype.

## G2. Trạm B41

Vị trí B41 hướng vào mô đất lớn và mục tiêu giả lập xe thiết giáp.

> **Sơn:** “Phía sau trống chưa?”  
> **Lâm:** “Trống.”  
> **Sơn:** “Phải tự nhìn. Đừng chỉ trả lời.”

Người chơi:

1. Nhặt B41.
2. Kiểm tra vùng phía sau bằng bố cục/camera cho phép quan sát.
3. Ngắm mô đất.
4. Bắn một phát.
5. Nhấn `R` để bắt đầu nạp quả tiếp theo.
6. Bắn mục tiêu giả lập xe thiết giáp.

Ý nghĩa câu chuyện: bài tập gieo từ sớm việc **nhận thức những người xung quanh**, không chỉ tập trung vào mục tiêu trước mặt.

### Trạng thái bắt buộc hay phụ

- Game Design Bible cũ ghi súng máy và B41 là **trạm phụ** để tránh tutorial quá tải.
- Flow mới nhất yêu cầu được hướng dẫn và bắn thử ở cả hai trường riêng, ngụ ý **bắt buộc**.
- **OPEN:** Cần khóa trước implementation.
- Khuyến nghị hiện tại: giữ cả hai là bắt buộc nhưng rất ngắn, mỗi trạm 1 chu kỳ thao tác hoàn chỉnh; có thể có thử thách điểm số tùy chọn sau đó.

### Checkpoint

`CP03_Firepower_Completed`

- Ghi sau khi hoàn thành block cover–trench–grenade và các trạm hỏa lực bắt buộc.

---

## H. Khu 5 — Gặp kíp xe 314

### Chuyển sang bãi cơ giới

Sau phần bộ binh, hướng dẫn viên/Sơn gọi Lâm và Quang sang nhận xe.

Lời thoại chuyển khu chưa khóa. Bản nháp:

> **Cán bộ thao trường:** “Lâm, Quang. Hai đồng chí sang bãi cơ giới nhận nhiệm vụ.”

Quang dẫn Lâm đi qua:

- Một hàng xe đang sửa chữa.
- Tiếng động cơ nổ thử.
- Tiếng búa và kim loại.
- Phụ tùng, bánh xích và thùng dụng cụ.

Không để xe 314 lộ ra ngay. Camera mở dần và cuối cùng mới cho thấy chiếc Type 59 hư cấu mang số **314**.

### Vị trí nhân vật khi reveal

- Hoàng nằm/ngồi dưới gầm hoặc cạnh xích, đang kiểm tra cơ cấu chạy.
- Khánh đứng bên tháp pháo.
- Quang và Lâm đi từ trái vào.

### Hội thoại giới thiệu kíp xe

> **Quang:** “Báo cáo, nạp đạn viên Quang và pháo thủ Lâm có mặt.”  
> **Hoàng:** “Đừng đứng lên xích. Tôi vừa chỉnh xong.”  
> **Khánh:** “Tôi là Khánh, trưởng xe. Người dưới gầm là Hoàng, lái xe.”  
> **Hoàng:** “Chưa chắc đã nhận đâu. Để xem cậu pháo thủ bắn thế nào.”  
> **Khánh:** “Từ hôm nay, đây là xe 314.”

### Khoảng nghỉ tương tác

Người chơi được tự do đi quanh xe trong một khoảng ngắn và tương tác với:

- Xích xe.
- Nòng pháo.
- Cửa lên xe.
- Số hiệu 314.
- Hoàng.
- Khánh.
- Quang.

Không cần animation khuôn mặt. Mỗi tương tác có thể chỉ là 1–2 dòng text.

### Lên xe

- Chỉ được lên xe tại vị trí cửa hợp lệ.
- Prompt: `[E] Lên xe 314`.
- Khi vào xe, HUD bộ binh chuyển sang HUD xe tăng.
- Camera vẫn nhìn ngang, không chuyển sang cockpit.

### Checkpoint

`CP04_Tank_Received`

- Ghi ngay trước hoặc sau lần đầu lên xe, sao cho reload checkpoint không bắt phát lại toàn bộ đoạn bộ binh.

---

## I. Khu 6 — Huấn luyện lái và bắn pháo

### Nguyên tắc điều khiển kíp xe

Người chơi trực tiếp điều khiển chiếc xe như một thể thống nhất, nhưng trong truyện:

- Khánh ra lệnh.
- Hoàng lái.
- Lâm điều khiển hỏa lực.
- Quang nạp đạn.

Điều này giải thích vì sao một người chơi dùng cùng bộ input nhưng hội thoại vẫn thể hiện công việc của bốn thành viên.

## I1. Bài lái cơ bản

> **Khánh:** “Hoàng điều khiển xe. Cậu quan sát đường và nghe lệnh.”  
> **Khánh:** “Tiến.”  
> **Khánh:** “Dừng.”  
> **Khánh:** “Lùi về cọc trắng.”

Người chơi học:

- `A / D`: chạy hai hướng trên trục ngang.
- Dừng đúng vị trí.
- Lùi về mốc.
- Vượt mô đất thấp.
- Đi chậm qua cầu gỗ tập nếu dùng trong layout.
- Không lao xe quá nhanh vào vật cản.
- `X`: bật/tắt động cơ nếu cơ chế này đã được prototype; trong tutorial có thể bắt đầu bằng một prompt rõ ràng.

**CUT:** Không có bài ziczac, cua nhiều làn hoặc chạy theo chiều sâu.

## I2. Điều khiển pháo và ngắm

Xe dừng tại vị trí bắn.

> **Khánh:** “Lâm, vào vị trí.”  
> **Khánh:** “Tìm bia. Chưa được khai hỏa.”

Người chơi:

- Di chuyển chuột lên/xuống để nâng/hạ nòng trong giới hạn cơ khí.
- Chuột phải bật/tắt chế độ ngắm.
- Đưa tâm ngắm vào bia.
- Chờ lệnh.

> **Khánh:** “Mục tiêu, bia xe tăng, hướng hai giờ.”  
> **Lâm:** “Đã thấy.”  
> **Khánh:** “Bắn.”

## I3. Nạp pháo thủ công

Sau phát đầu:

> **Quang:** “Pháo trống!”  
> **Khánh:** “Lâm, ra lệnh nạp.”  
> **Lâm:** “Nạp!”  
> **Quang:** “Đang nạp… xong!”

Quy tắc:

- Sau khi bắn, pháo không tự nạp.
- Người chơi nhấn `R` để Lâm phát lệnh và bắt đầu chu trình của Quang.
- Thời gian nạp baseline **7,5 giây** — PROTOTYPE.
- Không thể bắn khi đang nạp.
- Nếu bấm bắn khi pháo chưa nạp:

```text
PHÁO CHƯA NẠP
Nhấn R để ra lệnh nạp
```

HUD trạng thái:

```text
SẴN SÀNG → CHƯA NẠP → ĐANG NẠP → SẴN SÀNG
```

Hội thoại giải thích quy tắc chung:

> **Quang:** “Bắn xong nhớ báo tôi nạp.”  
> **Lâm:** “Tưởng cậu tự biết chứ?”  
> **Khánh:** “Tập cho rõ lệnh từ bây giờ.”

## I4. Đạn pháo AP/HE

- Thiết kế chung dùng hai loại đạn đơn giản: `AP` chống mục tiêu bọc thép và `HE` chống công sự/bộ binh.
- `V`: đổi loại đạn được chọn.
- Sau khi chọn loại, vẫn phải nhấn `R` để nạp.
- Loadout Màn 1 **PROTOTYPE:** `3 AP + 3 HE`.
- 34 viên là sức chứa tham khảo của xe, không phải lượng bắt buộc của nhiệm vụ.
- Nếu muốn đơn giản hóa hơn cho vertical slice, có thể tạm dùng một loại đạn trong prototype đầu, nhưng LDD ưu tiên giữ AP/HE vì bài sát hạch có cả công sự và mục tiêu bọc thép.

## I5. Súng máy đồng trục

- `2`: chọn súng máy đồng trục.
- `1`: trở lại pháo chính.
- Súng đồng trục dùng để bắn các bia bộ binh cơ khí.
- Có đạn sẵn sàng và đạn dự trữ; hết phần đang cấp phải nhấn `R`.
- Dây tham khảo khoảng 250 viên; lượng Màn 1 cần nhỏ hơn để ngăn spam.
- **OPEN:** Lượng đạn đồng trục cụ thể trong Màn 1.

## I6. Súng máy 12,7 mm trên nóc

- Sức chứa tham khảo của hệ thống chung là 200 viên, hộp khoảng 50 viên.
- **LOCKED cho scope Màn 1:** Không cần dạy 12,7 mm ở Màn 1. Nó liên quan tới đoạn Lâm phòng không trong campaign sau khi xe bị đứt xích và Khánh bị thương, không thuộc tutorial nhận xe hiện tại.

---

## J. Bài sát hạch tổng hợp

### Vai trò

- Đây là cao trào của Màn 1.
- Không phải boss fight và không có kẻ địch thật.
- Kiểm tra phối hợp, kỷ luật và các thao tác vừa học.
- Bài học cốt lõi: **“Bắn trúng chưa chắc đã là bắn đúng lúc.”**

### Chuỗi bài thi đề xuất

1. Bật động cơ nếu chưa bật.
2. Tiến đến khu chướng ngại.
3. Vượt mô đất thấp.
4. Đi chậm qua cầu gỗ tập, nếu asset/layout cho phép.
5. Dừng đúng giữa hai cọc trắng.
6. Lùi xe về sau mô đất theo lệnh.
7. Dùng súng đồng trục hạ ba bia bộ binh: bia gần, bia sau bao cát, bia trên mô đất.
8. Chọn `HE`, nhấn `R` và phá công sự giả.
9. Chọn `AP`, nhấn `R` và tìm bia xe thiết giáp di chuyển ngang trên ray.
10. Phát hiện mục tiêu trước khi nhóm bộ binh qua khỏi vùng nguy hiểm.
11. Giữ hỏa lực, chờ tín hiệu của Sơn.
12. Khai hỏa sau lệnh.
13. Đưa xe trở về bãi.

Nếu cần rút gọn, giữ tối thiểu:

- Tiến tới vạch bắn.
- Dừng đúng vị trí.
- Hạ bia công sự.
- Nạp pháo thủ công.
- Phát hiện bia xe tăng.
- Chờ bộ binh qua vùng nguy hiểm.
- Nhận lệnh rồi khai hỏa.
- Trở về bãi.

### Cao trào hội thoại

Lâm thấy bia sớm:

> **Lâm:** “Đã thấy mục tiêu.”  
> **Khánh:** “Giữ đạn.”  
> **Lâm:** “Em bắn được.”  
> **Khánh:** “Tôi biết. Bộ binh chưa qua hết.”

Biến thể từng được đề xuất: **“Người phía trước chưa qua hết.”** Bản “Bộ binh chưa qua hết” được ưu tiên vì liên kết trực tiếp với cơ chế phối hợp.

Nếu người chơi cố bắn sớm:

- Khóa an toàn thao trường ngăn phát bắn.
- Không để tutorial khiến người chơi giết đồng đội.
- Hiển thị:

```text
KHÔNG THỂ KHAI HỎA
Bộ binh còn trong vùng nguy hiểm
```

> **Khánh:** “Nhìn cả chiến trường, đừng chỉ nhìn tâm ngắm.”

Khi bộ binh qua đủ, nghe hai tiếng gõ lên thân xe:

> **Sơn:** “Đã qua đủ!”  
> **Khánh:** “314, khai hỏa.”

Sau phát bắn thành công, bài thi hoàn tất; không cần thêm một mục tiêu lớn hơn.

### Checkpoint

`CP05_Final_Test_Start`

- Ghi ngay trước bài sát hạch.
- Khi retry, reset toàn bộ bia, đạn tập, vị trí bộ binh, trạng thái xe và lệnh thoại về đầu bài.

---

## K. Kết màn, ảnh kíp xe và loading screen

### Trở về bãi

- Người chơi tự lái xe về bãi.
- Xe dừng ở vị trí quy định.
- Người chơi tắt động cơ bằng `X` nếu cơ chế đã được dạy; nếu chưa, dùng scripted command nhưng vẫn giữ camera gameplay.
- Hoàng kiểm tra xích.
- Khánh ghi kết quả.
- Quang gọi mọi người đứng cạnh xe để chụp ảnh.

### Hội thoại kết màn

> **Khánh:** “Từ hôm nay, bốn người một xe.”  
> **Hoàng:** “Lên thì nhớ chùi bùn ở giày.”  
> **Quang:** “Anh nói với người hay nói với xe đấy?”  
> **Hoàng:** “Cả hai.”

Lời kết của Khánh:

> **Khánh:** “Nghỉ đi. Ngày mai chúng ta học cách đưa nó ra khỏi thao trường.”

### Vật lưu niệm

Mở khóa:

```text
Ảnh kíp xe 314 — Đồng Lau
```

Ảnh có thể là illustration tĩnh trong Journal; không cần dựng animation chụp ảnh phức tạp.

### Nhật ký của Quang

Bản nháp:

> “Hôm nay chúng tôi nhận xe. Khánh ít nói, Hoàng nói chuyện với máy móc, còn Lâm nhìn khẩu pháo lâu hơn nhìn chúng tôi.”

Nội dung này xuất hiện ở loading screen hoặc Journal sau màn.

### Save cuối màn

Ghi:

- `mission01_completed = true`
- `opening_truck_completed = true`
- `current_mission = 2`
- mở Màn 2 trong Chọn màn
- mở ảnh kíp xe trong Nhật ký
- đổi trạng thái Main Menu từ truck opening sang xe 314 tại doanh trại

### Loading screen sau màn

Phong cách tổng thể đã định hướng theo màn hình tải quân sự cổ điển:

- Bàn tác chiến/tài liệu tối màu.
- Bản đồ hoặc tuyến hành trình.
- Ảnh kíp xe vừa mở khóa.
- Lá thư, hồi ký hoặc trang nhật ký của Quang.
- Một thanh tiến trình.
- Khi load hoàn tất: `TẢI XONG — NHẤN PHÍM ĐỂ TIẾP TỤC`.

Loading screen không chỉ chờ tải mà còn ghi lại cảm nhận và tâm trạng của nhân vật qua mỗi màn.

---

# 9. Bảng điều khiển Màn 1

| Thao tác | Điều khiển | Quy tắc |
|---|---|---|
| Di chuyển | **A / D** | Đi trái/phải; trên tăng tương ứng chạy theo hai hướng. |
| Chạy | **Shift** | Chỉ áp dụng khi đi bộ. |
| Cúi / núp thấp | **Ctrl** | Vật chắn thật sự chặn đạn; cúi không làm nhân vật bất tử. |
| Vượt vật cản thấp | **Space** khi đứng gần | Chỉ vượt vật cản được thiết kế cho thao tác này. |
| Bật / tắt ngắm | **Chuột phải** | Toggle: nhấn một lần bật, nhấn lần nữa tắt. |
| Bắn | **Chuột trái** | Bị khóa trong lúc nạp đạn hoặc tương tác. |
| Nạp đạn | **R** | Luôn thủ công. |
| Ném lựu đạn | **G** | Giữ để chọn lực, thả để ném; có đường dự báo trong tutorial. |
| Tương tác | **E** | Nhận đồ, dùng/rời súng cố định, lên/xuống xe ở vị trí cho phép. |
| Đổi SEMI/AUTO | **B** | Chỉ cho AK và vũ khí hỗ trợ tương thích; không có burst riêng. |
| Nâng / hạ nòng | **Chuột lên/xuống** | Nòng đi theo hướng ngắm trong giới hạn của xe. |
| Pháo chính | **1** | Chọn vũ khí chính của xe. |
| Súng máy đồng trục | **2** | Chọn đồng trục. |
| Đổi AP/HE | **V** | Chọn loại đạn, sau đó phải nhấn R để nạp. |
| Bật/tắt động cơ | **X** | Dùng cho xe; có thể đơn giản hóa trong prototype đầu. |
| Mục tiêu/tài nguyên | **Tab** | Xem objective và lượng đạn dự trữ. |

### Điều khiển không dùng trong Màn 1

- `Q` ra hiệu cho bộ binh tiến lên là cơ chế campaign sau; bài cuối Màn 1 dùng lệnh scripted của Khánh/Sơn để giới thiệu ý tưởng phối hợp, chưa bắt người chơi quản lý tổ bộ binh đầy đủ.

### Xung đột đã giải quyết

- Một tài liệu cũ ghi `C` để cúi.
- Yêu cầu mới hơn của chủ dự án ghi `Ctrl`.
- **LOCKED:** dùng `Ctrl`.
- `C` là **REPLACED** và phải được sửa trong Game Design Bible khi đồng bộ tài liệu lần tới.

---

# 10. Quy tắc manual reload xuyên suốt

Manual reload là một quy tắc bản sắc, không chỉ là prompt tutorial.

| Vũ khí | Quy tắc |
|---|---|
| AK | Hết băng không tự thay; nhấn `R`. Băng đang dùng và dự trữ tách riêng. |
| B41 | Sau mỗi phát phải nhấn `R` để bắt đầu nạp quả tiếp theo. |
| Pháo tăng | Sau phát bắn, nhấn `R` để Lâm ra lệnh Quang nạp; có thời gian nạp. |
| Súng máy cố định | Hết dây/hộp sẵn sàng thì nhấn `R`; reserve tách riêng. |
| Súng máy đồng trục | Hết phần đang cấp thì nhấn `R`; không tự lấy từ reserve. |

Không được thêm auto-reload ngầm khi chuyển vũ khí, bước vào trigger hoặc sau một khoảng chờ.

---

# 11. Tài nguyên và đạn dược Màn 1

## 11.1. Bộ binh

| Tài nguyên | Số lượng |
|---|---:|
| AK — băng trên súng | 30 viên |
| AK — dự trữ | 90 viên / 3 băng |
| Lựu đạn tập | 2 quả |

- Trang bị được cấp tại bàn cấp phát.
- Không có bắn tự do trước lệnh của Sơn.
- Các trạm tập có thể cấp lại đạn huấn luyện khi reset bài.
- Không dạy medkit/hồi máu trong Màn 1 vì không có sát thương thật. Hệ thống cứu thương và vật phẩm rải map thuộc các màn chiến đấu sau.

## 11.2. Xe tăng

| Tài nguyên | Số lượng/ghi chú |
|---|---|
| Pháo chính | **3 AP + 3 HE** — PROTOTYPE |
| Đồng trục 7,62 mm | Chưa chốt; dùng lượng tập nhỏ, có ready belt + reserve |
| 12,7 mm nóc xe | Không dùng trong Màn 1 |

Thông số tham khảo toàn dự án:

- Xe Type 59 có sức chứa tham khảo khoảng 34 viên pháo 100 mm, nhưng từng nhiệm vụ không cần cấp đủ.
- Tổng đạn 7,62 mm thực tế tham khảo 3.000–3.500 viên là quá lớn cho gameplay; phải chia theo nhiệm vụ/checkpoint để tránh spam.
- Súng 12,7 mm tham khảo 200 viên, hộp 50 viên.
- Dây 7,62 mm tham khảo khoảng 250 viên.

## 11.3. Refill

- Game chung refill tại checkpoint/điểm tiếp tế.
- Trong Màn 1, bàn cấp phát và từng trạm huấn luyện đóng vai trò nguồn đạn tập.
- Reset bài được phép phục hồi đúng loadout của bài, không coi là refill tự do.
- Cuối Màn 1 không mang nguyên lượng đạn tập sang Màn 2; Màn 2 nhận loadout nhiệm vụ tại điểm tiếp tế.

---

# 12. HUD, prompt và phản hồi

## 12.1. HUD bộ binh

Chỉ xuất hiện sau bàn cấp phát:

- Loại vũ khí.
- Đạn trong băng / dự trữ.
- SEMI/AUTO.
- Số lựu đạn.
- Trạng thái ngắm.
- Objective ngắn.
- Prompt tương tác.

## 12.2. HUD xe tăng

- Vũ khí đang chọn: pháo chính/đồng trục.
- Loại đạn được chọn: AP/HE.
- Số viên còn lại theo loại.
- Trạng thái nòng: Sẵn sàng/Chưa nạp/Đang nạp.
- Đạn đồng trục ready/reserve.
- Tình trạng động cơ/xích nếu hệ thống đã có.
- Objective và vùng dừng.

## 12.3. Phản hồi tutorial

- Prompt ngắn, không che nhân vật.
- Sơn/Khánh xác nhận bằng text để tutorial gắn với nhân vật.
- Bia trúng có animation hạ xuống và âm thanh cơ khí.
- Điểm xạ: hiện `TỐT`, `ĐẠT` hoặc `CHƯA ĐẠT`.
- Khi sai, hướng dẫn lại thao tác; không spam popup dài.

---

# 13. Checkpoint, save và retry

## 13.1. Checkpoint đề xuất

| ID | Vị trí | Dữ liệu cần khôi phục |
|---|---|---|
| `CP01_Arrival_LamGrounded` | Lâm xuống xe | Opening complete, vị trí sân đón, chưa cấp vũ khí hoặc đúng trạng thái sau xuống xe |
| `CP02_AK_Completed` | Hoàn thành AK | Trang bị còn lại, tutorial flags, target state |
| `CP03_Firepower_Completed` | Xong cover/lựu/MG/B41 | Đạn reset phù hợp, chuyển sang bãi xe |
| `CP04_Tank_Received` | Nhận/lên xe 314 | Tank state, crew dialogue flags, loadout training |
| `CP05_Final_Test_Start` | Trước sát hạch | Xe, bia, bộ binh, ammo và dialogue reset đầu bài |
| `MISSION01_COMPLETE` | Sau ảnh kíp xe | Mở Màn 2, Journal, menu progression |

Checkpoint cụ thể có thể điều chỉnh để giữ tổng thời lượng, nhưng phải bảo đảm người chơi không phải phát lại opening hoặc toàn bộ tutorial sau một lỗi cuối màn.

## 13.2. Quy tắc save toàn dự án áp dụng ở Màn 1

- Một `CampaignSave` chính và một backup.
- Autosave tại checkpoint.
- Không manual save giữa combat/bài test.
- Failure không ghi đè checkpoint tốt bằng trạng thái thất bại.
- Nếu save chính hỏng, thử backup.
- Replay từ Chọn màn chạy trong `ReplaySession`, không ghi đè tiến trình campaign chính.
- `GlobalSettings` tách khỏi `CampaignSave`.

## 13.3. Fail/retry trong tutorial

Màn 1 nên rất ít hard fail:

- Ném lựu đạn sai: reset trạm.
- Bắn sai nhịp: cho thử bia tiếp theo hoặc lặp bài ngắn.
- Hết đạn tập do thử nghiệm: cán bộ cấp lại đúng lượng của trạm.
- Lái quá vạch: yêu cầu lùi lại hoặc reset vị trí gần nhất.
- Cố bắn khi bộ binh chưa qua: safety lock chặn phát bắn.
- Xe mắc vật cản do bug/layout: reset về marker gần nhất.
- Không có thương vong thật trong Màn 1.

---

# 14. Hội thoại, radio và localization

## 14.1. Hình thức

- Không voice.
- Text subtitle/hộp thoại nhỏ trong gameplay.
- Mỗi lượt 1–2 câu ngắn.
- Các đoạn dài chỉ xuất hiện ở Journal/loading screen.
- Hội thoại không khóa điều khiển trừ khi cần đảm bảo an toàn tutorial.

## 14.2. Localization từ ngày đầu

- Tiếng Việt là source locale.
- Tất cả hội thoại, tutorial prompt, objective, menu, Journal và loading text phải dùng String Table.
- Dùng key ổn định, không lấy nguyên câu tiếng Việt làm key.
- Không dịch song song trong Phase 1; chỉ bảo đảm kiến trúc hỗ trợ ngôn ngữ.
- Tiếng Anh là ngôn ngữ mở rộng đầu tiên sau khi nội dung tiếng Việt khóa.
- Có thể thêm tiếng Nhật hoặc ngôn ngữ khác về sau.
- UI phải co giãn cho chuỗi dài hơn tiếng Việt.

Ví dụ key:

```text
mission01.title
mission01.objective.report_ready
mission01.dialogue.quang.intro_01
mission01.dialogue.son.armor_01
tutorial.move
tutorial.reload
tutorial.fire_mode
tutorial.tank_reload
loading.mission01.journal
```

## 14.3. Chữ trên asset môi trường

- Ưu tiên bảng trắng/blank plate + TextMeshPro.
- Asset B2 đã có cả cổng không chữ, sign tách lớp và bản ghép hoàn chỉnh.
- Cổng doanh trại có thể giữ tiếng Việt như tên riêng môi trường nếu chủ dự án duyệt; tuy nhiên bản split vẫn tốt hơn cho bản tiếng Anh.
- Không bake hội thoại hoặc hướng dẫn vào PNG.

---

# 15. Âm thanh

## 15.1. Đoạn đường quê

- Động cơ xe cam nhông.
- Bánh xe trên đường đất.
- Gió, chim, tiếng đồng quê.
- Radio mono/rè, âm lượng nhỏ.
- Tiếng nói cười của tân binh có thể là ambience không rõ lời; nội dung rõ vẫn dùng text.

## 15.2. Doanh trại và thao trường

- Tiếng bước chân, vải, dây trang bị.
- AK semi và loạt 2–3 viên phải nghe rõ khác biệt.
- Tiếng bia cơ khí bật/hạ.
- Tiếng lựu đạn tập, súng máy, B41.
- Mỗi khu có khoảng lặng chuyển tiếp để tránh fatigue.

## 15.3. Bãi xe

- Động cơ diesel.
- Búa, xích và kim loại.
- Tiếng lên/xuống xe.
- Tiếng khóa nòng pháo, Quang thao tác nạp.
- Hai tiếng gõ lên thân xe làm tín hiệu bộ binh đã qua.

---

# 16. Background và art direction

## 16.1. Thời tiết/ánh sáng

- Màn 1: trời sáng, nắng dịu.
- Không dùng mưa trong Màn 1.
- Mưa vẫn có thể xuất hiện ở các màn khác dưới dạng weather overlay; không cần sản xuất cho LDD-01.

## 16.2. Parallax mở đầu

Các lớp gợi ý:

1. Sky.
2. Núi xa.
3. Làng/đồng lúa.
4. Hàng cây trung cảnh.
5. Đường gameplay.
6. Tre/cỏ tiền cảnh.
7. Bụi và atmosphere.

## 16.3. Art direction chung

- Painted 2D, giàu không khí nhưng silhouette gameplay phải rõ.
- Nhân vật và mục tiêu cần ít chi tiết hơn background để đọc tốt khi di chuyển.
- Xe 314 và xe cam nhông cần khớp tông với Main Menu hiện tại.
- Tránh làm texture quá chi tiết khiến một solo developer khó duy trì animation nhất quán.

---

# 17. Asset đã có và asset còn cần

## 17.1. Gói B1 — Truck Opening Asset Pack

**Đã có:** `314_TruckOpening_Asset_Pack_B_v1.zip`

Nội dung đã xác nhận:

- Sky.
- Núi xa.
- Làng.
- Đồng lúa.
- Chuyển cảnh bìa rừng.
- Đường looping.
- Tre/cỏ foreground.
- Thân xe cam nhông không bánh.
- Bánh trước và bánh sau tách riêng.
- Tân binh ngồi trên xe.
- Tài xế.
- Nông dân/trâu.
- Props nông thôn.
- Dust animation frames/sheet.
- Blank training sign.
- Preview và tài liệu Unity.

Mục đích: Main Menu lần đầu và phần đầu tuyến đường tới doanh trại.

## 17.2. Gói B2 — Training Camp Arrival v1.1

**Đã có:** `314_TruckOpening_Asset_Pack_B2_Training_Camp_Arrival_v1.1.zip`

Nội dung đã xác nhận:

- Cổng doanh trại bản không chữ và bản có tên.
- Biển đỏ weathered tách riêng.
- Sao vàng tách riêng.
- Layer chữ chính xác.
- Composite text “DOANH TRẠI / QUÂN ĐỘI NHÂN DÂN VIỆT NAM”.
- Watchtower và fence.
- Training yard midground.
- Tents/crates.
- Target row.
- Training trench.
- Obstacle course.
- Truck stop pad.
- Lâm xuống xe: sequence 4 frame và frame rời.
- Tân binh xuống xe: 3 pose và frame rời.
- Instructor waiting.
- Preview và Unity docs.

Chuỗi asset B2 mong muốn:

```text
Village → Rice Field → Forest Edge → Gate → Training Yard
→ Truck Stop → Lam Disembarks → Player Control
```

B2 tái sử dụng truck, wheel, driver, road, background và dust từ B1; không nhân đôi.

## 17.3. Main Menu hiện tại

- Scene menu với xe 314 tĩnh tại doanh trại đã được dựng và nối nút.
- Asset này vẫn có giá trị:
  - làm trạng thái menu sau khi hoàn thành Màn 1;
  - làm fallback nếu opening truck chưa được tích hợp;
  - làm base cho các trạng thái campaign sau.

## 17.4. Asset cần bổ sung/kiểm tra

Danh sách dưới đây chưa được xác nhận đầy đủ trong project hiện tại:

### Nhân vật

- Lâm: idle, walk, run, crouch, aim, fire, reload, vault, grenade, interact, disembark.
- Quang: walk, idle, dialogue, lên xe.
- Sơn: idle, walk, chỉ dẫn, bắn minh họa/gõ thân xe.
- Khánh: idle cạnh tháp pháo, command poses.
- Hoàng: nằm/ngồi kiểm tra xích, đứng, lên xe.
- Sĩ quan tiếp nhận, cán bộ cấp phát, hướng dẫn viên.
- Tân binh xếp hàng.
- Nhóm bộ binh trong bài sát hạch.

### Vũ khí và thao trường

- AK và animation SEMI/AUTO/reload.
- Lựu đạn tập và arc indicator.
- Súng máy cố định, mount và belt/box.
- B41, đạn và backblast VFX.
- Bia cơ khí bật/hạ.
- Bia bộ binh sau bao cát.
- Mục tiêu công sự giả.
- Mục tiêu xe thiết giáp nhìn ngang chạy trên ray.
- Mô đất, sandbag, hàng rào, bảng phân khu.
- Cổng gỗ nội bộ “THAO TRƯỜNG”.
- Vạch bắn, cọc trắng, cầu gỗ tập.

### Xe 314

- Thân xe, turret, nòng và bánh/xích tách rig phù hợp.
- Forward/reverse track animation.
- Barrel elevation.
- Muzzle flash, smoke, impact.
- Hatch/entry point.
- Số hiệu 314.
- Coax firing VFX.
- AP/HE target impact.

### UI

- HUD bộ binh.
- Fire-mode indicator.
- Grenade count.
- Fixed-weapon UI.
- Tank HUD và loader state.
- Tutorial prompt panel.
- Objective panel.
- Crew photo Journal illustration.
- Loading screen Màn 1.

## 17.5. Cờ hiệu và biểu tượng

- Asset cổng B2 hiện dùng biển đỏ và ngôi sao vàng như một chi tiết trang trí/nhận diện, chưa phải quyết định cuối về cờ đơn vị.
- **OPEN:** Nếu treo cờ thật trong Màn 1, cần khóa theo bối cảnh hư cấu và vị trí thao trường: cờ đỏ sao vàng hay một cờ hiệu đơn vị hư cấu.
- Phương án “cờ giải phóng miền Nam” từng được đặt ra cho toàn campaign, nhưng chưa chốt và không nên tự động đưa vào doanh trại Màn 1 khi mốc thời gian/địa lý chi tiết chưa khóa.
- Không dùng phù hiệu của một đơn vị lịch sử có thật cho đơn vị xe 314.

---

# 18. Mission logic và trigger gợi ý

| Trigger/flag | Tác dụng |
|---|---|
| `menu.start_journey` | Bắt đầu fade menu và chuyển loop thành route |
| `ArrivalGateEntered` | Đổi ambience sang doanh trại |
| `TruckBrakeStart` | Xe giảm tốc |
| `TruckStop` | Bắt đầu disembark sequence |
| `LamGrounded` | Bật player control, save CP01 |
| `briefing.completed` | Cho phép theo hướng dẫn viên |
| `equipment.issued` | Hiện HUD và loadout 30/90 + 2 |
| `movement.completed` | Mở cổng sang trường AK |
| `ak.semi.completed` | Mở manual reload lesson |
| `ak.reload.completed` | Mở AUTO/point-fire lesson |
| `ak.completed` | Save CP02 |
| `grenade.completed` | Cho phép đi sang bãi hỏa lực |
| `fixed_mg.completed` | Đánh dấu trạm MG |
| `b41.completed` | Đánh dấu trạm B41 |
| `firepower.completed` | Save CP03 và gọi sang bãi xe |
| `tank.revealed` | Mở interaction quanh 314 |
| `tank.entered` | Chuyển HUD, save CP04 |
| `tank.drive.completed` | Mở gunnery |
| `tank.reload.completed` | Mở sát hạch |
| `final_test.started` | Save CP05 |
| `infantry.clear` | Cho phép pháo khai hỏa mục tiêu cuối |
| `mission01.completed` | Save end, unlock M2/menu/journal |

---

# 19. Điều kiện hoàn thành và acceptance criteria

Màn 1 được coi là đạt khi:

1. Người chơi mới hoàn thành trong khoảng 18–20 phút, không tính thời gian đứng yên tùy ý.
2. Luồng từ Main Menu truck loop sang gameplay diễn ra không có loading/cutscene gây cảm giác tách rời, trừ khi giới hạn kỹ thuật buộc dùng fade rất ngắn.
3. Opening chỉ phát lại khi chưa hoàn thành hoặc khi bắt đầu hành trình mới.
4. Camera luôn tuân thủ gameplay 2D nhìn ngang.
5. Không có bài lái ziczac hoặc chiều sâu.
6. Người chơi được giới thiệu nhân vật trước khi bị yêu cầu bắn.
7. AK dạy đủ SEMI, AUTO, điểm xạ và manual reload.
8. Bia AK đọc rõ ở góc ngang và dùng animation cơ khí.
9. Cover dựa vào collider/vị trí, không phải trạng thái bất tử.
10. Chiến hào là địa hình thấp, không phải mode riêng.
11. Súng máy và B41 nằm ở hai trường bắn riêng.
12. Manual reload hoạt động nhất quán trên AK, B41, súng máy và pháo tăng.
13. Người chơi gặp và phân biệt được Khánh, Hoàng, Quang, Sơn và Lâm.
14. Xe 314 được reveal có nhịp, không xuất hiện ngay khi bước vào bãi cơ giới.
15. Tutorial xe có tiến, lùi, dừng, nâng/hạ nòng, đồng trục, AP/HE và nạp pháo.
16. Bài cuối buộc người chơi chờ bộ binh an toàn trước khi bắn.
17. Cố bắn sớm không giết đồng đội mà kích hoạt safety feedback.
18. Màn kết thúc bằng ảnh kíp xe, Journal và save tiến trình.
19. Sau màn, Main Menu đổi sang xe 314 tại doanh trại.
20. Tất cả text dùng localization key; tiếng Việt là source locale.
21. Continue và checkpoint không phát lại nội dung dài đã hoàn thành.
22. Không sửa/xóa tiến trình campaign khi replay Màn 1 từ Chọn màn.

---

# 20. Danh sách quyết định đã khóa

- Tên game hiện dùng: **314**.
- Màn 1: **Ngày nhận xe** tại thao trường **Đồng Lau** hư cấu.
- 18–20 phút.
- Mở bằng xe cam nhông trên đường quê và nối từ Main Menu.
- Lần đầu mới có truck menu/opening; sau Màn 1 menu đổi sang xe 314.
- 2D nhìn ngang, chủ yếu trái sang phải.
- Không cutscene, không voice.
- Có hội thoại giới thiệu trước khi bắn.
- AK 30/90, 2 lựu đạn tập.
- AK có SEMI/AUTO, không có burst riêng.
- Manual reload xuyên suốt.
- `Ctrl` để cúi, RMB toggle aim, LMB bắn, `R` nạp, `G` lựu đạn, `E` tương tác.
- Chiến hào là địa hình.
- Súng máy và B41 ở hai khu riêng.
- Gặp Sơn trước, sau đó gặp Khánh/Hoàng/xe 314.
- Người chơi trực tiếp điều khiển xe, hội thoại vẫn thể hiện vai trò của cả kíp.
- Không có lái ziczac.
- Cao trào là bài sát hạch phối hợp, không phải mini-boss.
- Phải chờ bộ binh qua khỏi vùng nguy hiểm.
- Kết bằng ảnh kíp xe và Journal của Quang.
- Tiếng Việt trước, kiến trúc sẵn cho tiếng Anh/ngôn ngữ khác.

---

# 21. Những điểm còn mở trước production lock

| ID | Vấn đề | Trạng thái/khuyến nghị |
|---|---|---|
| `OPEN-01` | Bản nhạc radio và quyền sử dụng | Kiểm tra pháp lý; nếu chưa rõ dùng nhạc hành khúc nguyên bản. |
| `OPEN-02` | Câu khẩu quyết điểm xạ chính xác | Xác minh “ra tay” hay “da tay”; sau đó khóa String Table. |
| `OPEN-03` | MG/B41 bắt buộc hay phụ | Khuyến nghị bắt buộc nhưng mỗi trạm chỉ một chu kỳ ngắn. |
| `OPEN-04` | Lời sĩ quan tiếp nhận | Duyệt bản nháp hoặc viết lại theo tone ít khẩu hiệu, gần gũi. |
| `OPEN-05` | Số đạn súng máy tập | Prototype để đủ một lần reload nhưng không đủ spam dài. |
| `OPEN-06` | Giữ G chọn lực hay lực cố định | Hiện ưu tiên giữ/thả; cần kiểm tra cảm giác điều khiển. |
| `OPEN-07` | Có dạy bật/tắt động cơ X ngay Màn 1 không | Nên dạy khi bắt đầu/khép lại bài xe nếu không gây quá tải. |
| `OPEN-08` | Dùng AP/HE ngay prototype đầu hay một loại đạn | LDD ưu tiên AP/HE; có thể tạm giản lược ở bản kỹ thuật đầu. |
| `OPEN-09` | Cổng doanh trại dùng chữ bake hay TMP | Ưu tiên asset tách + TMP để localization. |
| `OPEN-10` | Checkpoint sau movement hay sau AK | Hiện chọn sau AK; playtest thời gian retry. |
| `OPEN-11` | Cờ treo tại doanh trại | Chưa chốt; tránh tự gán cờ lịch sử khi mốc/địa lý chưa khóa, có thể dùng cờ hiệu đơn vị hư cấu. |

---

# 22. Những ý tưởng đã thay thế hoặc bị loại

| Ý tưởng cũ | Trạng thái | Phương án hiện tại |
|---|---|---|
| Menu ngay từ đầu chỉ có xe tăng 314 | REPLACED một phần | Lần đầu là truck loop; xe tăng menu dùng sau khi nhận xe và ở các trạng thái campaign sau. |
| Mở đầu bằng “Tiến quân ca” và hiển thị lời | REPLACED | Radio hành khúc/“Đoàn Vệ quốc quân” ngắn, chờ xử lý quyền sử dụng. |
| Vào game là bắn ngay | CUT | Có đoạn xe, xuống xe, trình diện, di chuyển và giới thiệu nhân vật trước. |
| Lái tăng qua đường ziczac | CUT | Chỉ tiến/lùi, dừng mốc, mô đất/cầu trên trục ngang. |
| `C` để cúi | REPLACED | `Ctrl`. |
| Một trường bắn chung cho AK/MG/B41 | CUT | Ba khu riêng, đặc biệt MG và B41 tách rõ. |
| Bia giấy quay mặt ra camera | REPLACED | Bia cơ khí silhouette nhìn ngang. |
| Tutorial cuối là mini-boss thật | CUT | Bài sát hạch phối hợp với mục tiêu cơ khí. |
| Auto reload | CUT | Manual reload mọi vũ khí phù hợp. |
| Phát lại opening mỗi lần Continue | CUT | Chỉ phát khi New Journey hoặc chưa hoàn thành opening. |

---

# 23. Ghi chú triển khai Unity

Tài liệu này không yêu cầu viết code ngay, nhưng khi chuyển sang implementation nên chia theo hệ thống:

1. `OpeningTruckSequence`
   - menu loop state;
   - fade UI;
   - route timeline/state machine;
   - B1/B2 parallax và markers;
   - save flag `opening_truck_completed`.

2. `TutorialDirector_Mission01`
   - quản lý state A–K;
   - objective;
   - khóa/mở input;
   - checkpoint;
   - localized dialogue events.

3. `InfantryTutorialStations`
   - movement;
   - AK;
   - cover/trench;
   - grenade;
   - fixed MG;
   - B41.

4. `TankTutorialStations`
   - drive markers;
   - weapon selection;
   - loader state;
   - AP/HE;
   - final-test safety zone.

5. `CampaignSaveManager`
   - CampaignSave/GlobalSettings tách biệt;
   - checkpoint snapshot;
   - backup;
   - New Journey modal;
   - ReplaySession.

6. `Localization`
   - String Tables tiếng Việt;
   - TMP;
   - không hard-code câu thoại trong MonoBehaviour.

7. `Mission01LoadingScreen`
   - ảnh kíp xe;
   - journal;
   - progress;
   - press-to-continue.

## 23.1. Hiện trạng project trước khi triển khai LDD-01

Các hạng mục nền tảng đã được tạo trong project:

- `MainMenuController.cs`
- `MenuPanel.cs`
- `MenuSaveProvider.cs`
- `MainMenuSceneBuilder.cs`
- `MainMenuAssetPackApplier.cs`
- `MenuAmbience.cs`
- `MenuButtonVisual.cs`
- Scene `MainMenu.unity` đã được tạo, nút và panel đã được wire bằng Editor command.
- Scene `Level01_Training` hiện mới là placeholder routing, chưa có gameplay Màn 1.
- Main Menu đã áp dụng asset xe 314 tĩnh và hiển thị được trong Play Mode.
- `Continue` hiện phải giữ disabled vì chưa có save provider thực tế nối vào `MenuSaveProvider`.
- Nhánh Git từng được đẩy lên cho nền tảng player là `M1-player-foundation`; cần kiểm tra/merge trạng thái nhánh trong repository thật trước khi tiếp tục code, không dựa riêng vào ghi chú này.
- Các script menu, player hiện có và project settings cần được bảo toàn khi triển khai Màn 1.

Việc tiếp theo về kỹ thuật không phải dựng lại menu từ đầu mà là:

1. Tích hợp B1/B2 thành trạng thái menu/opening trước save.
2. Xây `CampaignSaveManager` và adapter cho `MenuSaveProvider`.
3. Thay placeholder `Level01_Training` bằng blockout theo flow A–K.
4. Giữ artwork xe 314 hiện tại làm trạng thái menu sau `mission01_completed`.

---

# 24. Tài liệu tham khảo được nhắc trong trao đổi

Các link sau chỉ là tư liệu tham khảo cho kỹ thuật điểm xạ, không biến nhân vật/trận đánh trong game thành sự kiện có thật:

- Dân Việt — bài viết về kỹ thuật bắn điểm xạ AK:  
  <https://danviet.vn/cach-ban-nao-cua-bo-doi-viet-nam-lam-cho-dich-nghe-tieng-dan-da-phai-kieng-de-d1328278.html>
- Báo Quân đội nhân dân — tư liệu có nhắc tiếng điểm xạ AK hai phát:  
  <https://www.qdnd.vn/50nam-tong-tien-cong-va-noi-day-xuan-mau-than1968/danh-gia-phan-tich/nhung-vu-khi-noi-tieng-cua-khoi-xhcn-trong-cuoc-tong-tien-cong-va-noi-day-xuan-mau-than-1968-530384>

Các mô tả kiểu “nghe tiếng súng là khiếp vía” chỉ nên xem là giai thoại/chất liệu không khí, không đưa vào game như một khẳng định lịch sử. Cách dùng tốt hơn là biến nhịp điểm xạ thành dấu hiệu đồng đội nhận ra Lâm.

---

# 25. Tóm tắt một câu

**Màn 1 “Ngày nhận xe” đưa Lâm từ một xe cam nhông chạy qua miền quê thanh bình tới thao trường Đồng Lau, dạy người chơi chiến đấu bộ binh và vận hành xe tăng 314, rồi kết thúc bằng bài học rằng một phát bắn đúng không chỉ cần trúng mục tiêu mà còn phải bảo vệ những người đang tiến lên phía trước.**
