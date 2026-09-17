# Generation Prompts — Gói B v1

Các ảnh raster trong gói được tạo bằng built-in image generation workflow. Gói A Main Menu được dùng làm tham chiếu art direction; visual target của Gói B được dùng làm tham chiếu cho các asset còn lại.

## Style block dùng chung

```text
2D hand-painted realistic-stylized game art, strict orthographic side-scrolling camera,
grounded mid-20th-century rural Vietnamese atmosphere, muted olive / warm gold / blue-grey palette,
soft dawn light from the right, crisp readable silhouettes, no real unit insignia, no readable text,
no logo, no watermark. Transparent assets must use genuine RGBA alpha with no checkerboard baked in.
```

## B00 — Visual target

```text
A wide 16:9 concept frame of a fictional olive-drab canvas-bed military cargo truck carrying young recruits,
travelling left-to-right through peaceful ripe rice fields. Include a small tiled-roof village, layered hills,
farmers, one water buffalo and bamboo foreground framing. Exact clean side profile; truck near the lower center;
clear horizontal layers for parallax; calm hopeful dawn; no UI or text.
```

## B01–B07 — Environment layers

```text
B01: dawn sky only; pale blue-grey to peach/gold gradient, thin clouds, low sun to the right; opaque; no land.
B02: distant layered blue-green tropical hills with mist; transparent above the ridge; no buildings or field.
B03: sparse rural village strip with old red tile roofs, banana, palms, hedges, bamboo and utility poles; RGBA.
B04: continuous ripe rice-paddy strip with bunds, irrigation reflections and detailed near stalks; RGBA above field.
B05: dirt-and-gravel roadside tile with damp horizontal wheel ruts; overlap-friendly left/right edges; opaque.
B06: dark foreground grass, broad leaves, bamboo and low fence fragments; center kept open for the truck; RGBA.
B07: countryside-to-forest transition, sparse vegetation on the left becoming dense jungle on the right; RGBA.
```

## B08–B12 — Vehicle rig

```text
B08: one fictional generic mid-century cargo truck, exact left-side profile facing right; cab, chassis, bed and
canvas intact; both road wheels removed; wheel wells and windows transparent; open canvas side; no occupants.
B09: one front wheel, exact circular side profile, weathered rubber and olive six-lug hub; transparent.
B10: one rear wheel, matching materials, slightly flatter eight-lug hub and heavier dirt; transparent.
B11: seven seated young Vietnamese recruits in olive uniforms and plain pith-style helmets, one horizontal row,
varied calm poses, shaded as if under canvas; no truck parts or background; transparent.
B12: one seated Vietnamese driver, side profile facing right, hands on an invisible steering wheel, sized for
the cab window; no chair, wheel or vehicle parts; transparent.
```

## B13–B16 — Ambient sprites and VFX

```text
B13: exactly four separated sprites in one row: farmer holding rice, farmer harvesting with a small sickle,
farmer carrying baskets on a shoulder pole, and one walking water buffalo; full body, same baseline; transparent.
B14: exactly six separated rural props in a 3x2 grid: haystack, bamboo fence, baskets, handcart, banana plants,
blank stone marker; no labels or background; transparent.
B15: exactly eight dust-puff frames progressing from small burst to expansion and dissipation, arranged 4x2;
soft tan-grey translucent painted VFX, transparent outside the effect. Final sheet normalized to 2048x1024.
B16: blank weathered wooden training-ground sign on two posts, flat front face for TextMeshPro overlay; no text;
transparent.
```

Post-processing was limited to alpha cleanup, trimming, deterministic crops, sheet normalization and preview compositing. No background or subject was repainted outside the image-generation workflow.
