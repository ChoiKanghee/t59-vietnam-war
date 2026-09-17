#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
B1="$ROOT/314_TruckOpening_Asset_Pack_B"
B2="$ROOT/314_TruckOpening_Asset_Pack_B2_Training_Camp_Arrival"
OUT="$B2/Preview"
TMP="$(mktemp -d)"
trap 'rm -rf "$TMP"' EXIT

make_yard_base() {
  local output="$1"
  convert "$B1/Background/B01_sky_dawn_1920x1080.png" -resize 1920x1080\! \
    \( "$B1/Background/B02_mountains_far.png" -resize 1920x632 \) -geometry +0+70 -composite \
    \( "$B2/Background/B2_04_training_yard_midground.png" -resize 1920x768 \) -geometry +0+312 -composite \
    \( "$B2/Props/Training/B2_05a_tents_crates.png" -resize 430x276 \) -geometry +55+430 -composite \
    \( "$B2/Props/Training/B2_05b_target_row.png" -resize 445x300 \) -geometry +1385+435 -composite \
    \( "$B2/Props/Training/B2_05c_training_trench.png" -resize 450x281 \) -geometry +880+500 -composite \
    \( "$B2/Props/Training/B2_05d_obstacle_course.png" -resize 390x253 \) -geometry +1150+490 -composite \
    "$output"
}

add_truck() {
  local input="$1"
  local output="$2"
  local x="$3"
  local y="$4"
  convert "$input" \
    \( "$B1/Vehicle/B11_recruits_row.png" -resize 650x147 \) -geometry +$((x+45))+$((y+95)) -composite \
    \( "$B1/Vehicle/B12_driver.png" -resize 139x200 \) -geometry +$((x+740))+$((y+95)) -composite \
    \( "$B1/Vehicle/B08_truck_body_no_wheels.png" -resize 1120x424 \) -geometry +${x}+${y} -composite \
    \( "$B1/Vehicle/B10_wheel_rear.png" -resize 226x226 \) -geometry +$((x+146))+$((y+250)) -composite \
    \( "$B1/Vehicle/B09_wheel_front.png" -resize 236x235 \) -geometry +$((x+812))+$((y+245)) -composite \
    "$output"
}

# Preview 1: the reused B1 truck crosses the new entrance assembly.
make_yard_base "$TMP/gate_base.miff"
convert "$TMP/gate_base.miff" \
  \( "$B2/Entrance/B2_02_watchtower.png" -resize 305x460 \) -geometry +85+410 -composite \
  \( "$B2/Entrance/B2_03_fence_module.png" -resize 760x369 \) -geometry +-150+620 -composite \
  \( "$B2/Entrance/B2_03_fence_module.png" -resize 760x369 \) -geometry +1310+620 -composite \
  \( "$B1/VFX/Dust/dust_04.png" -resize 310x206 \) -geometry +330+795 -composite \
  "$TMP/gate_with_dust.miff"
add_truck "$TMP/gate_with_dust.miff" "$TMP/gate_with_truck.miff" 360 520
convert "$TMP/gate_with_truck.miff" \
  \( "$B2/Entrance/B2_01e_gate_named_complete.png" -resize 1130x742 \) -geometry +390+210 -composite \
  "$TMP/gate_final.ppm"
ffmpeg -loglevel error -y -i "$TMP/gate_final.ppm" -frames:v 1 "$OUT/B2_10_preview_gate_1920x1080.png"

# Preview 2: truck stopped; Lam and another recruit are climbing down.
make_yard_base "$TMP/parked_base.miff"
convert "$TMP/parked_base.miff" \
  \( "$B2/Ground/B2_06_truck_stop_pad_overlay.png" -resize 1470x490 \) -geometry +120+620 -composite \
  "$TMP/parked_pad.miff"
add_truck "$TMP/parked_pad.miff" "$TMP/parked_truck.miff" 245 520
convert "$TMP/parked_truck.miff" \
  \( "$B2/Characters/Lam/B2_07b_lam_descend_02.png" -resize 294x330 \) -geometry +300+665 -composite \
  \( "$B2/Characters/Recruits/B2_08a_recruit_step_down.png" -resize 278x330 \) -geometry +535+668 -composite \
  \( "$B2/Characters/Staff/B2_09_instructor_waiting.png" -resize 220x340 \) -geometry +1540+650 -composite \
  "$TMP/parked_final.ppm"
ffmpeg -loglevel error -y -i "$TMP/parked_final.ppm" -frames:v 1 "$OUT/B2_11_preview_parked_1920x1080.png"

# Preview 3: Lam has landed and control can pass to the player.
make_yard_base "$TMP/start_base.miff"
convert "$TMP/start_base.miff" \
  \( "$B2/Ground/B2_06_truck_stop_pad_overlay.png" -resize 1450x483 \) -geometry +-80+630 -composite \
  "$TMP/start_pad.miff"
add_truck "$TMP/start_pad.miff" "$TMP/start_truck.miff" 65 535
convert "$TMP/start_truck.miff" \
  \( "$B2/Characters/Recruits/B2_08b_recruit_landing.png" -resize 260x338 \) -geometry +750+655 -composite \
  \( "$B2/Characters/Recruits/B2_08c_recruit_walk_right.png" -resize 242x330 \) -geometry +950+660 -composite \
  \( "$B2/Characters/Lam/B2_07d_lam_idle_right.png" -resize 258x345 \) -geometry +1190+650 -composite \
  \( "$B2/Characters/Staff/B2_09_instructor_waiting.png" -resize 220x340 \) -geometry +1540+650 -composite \
  "$TMP/start_final.ppm"
ffmpeg -loglevel error -y -i "$TMP/start_final.ppm" -frames:v 1 "$OUT/B2_12_preview_gameplay_start_1920x1080.png"
