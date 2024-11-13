using System;
using System.Collections;
using System.IO;
using System.Text;

public class StringController : UWEBase
{
	private struct huffman_node
	{
		public char symbol;

		public int parent;

		public int left;

		public int right;
	}

	private struct block_dir
	{
		public long block_no;

		public long address;

		public long NoOfEntries;
	}

	public static int str_it_looks_to_be_that_of_ = 22;

	public static int str_they_look_to_be_those_of_ = 23;

	public static int str_you_are_not_ready_to_advance_ = 24;

	public static int str_that_is_not_a_mantra_ = 25;

	public static int str_knowledge_and_understanding_fill_you_ = 26;

	public static int str_you_cannot_advance_any_further_in_that_skill_ = 27;

	public static int str_you_have_advanced_greatly_in_ = 28;

	public static int str_you_have_advanced_in_ = 29;

	public static int str_none_of_your_skills_improved_ = 30;

	public static int str_you_cannot_advance_in_ = 34;

	public static int str_to_the_north = 36;

	public static int str_to_the_northeast = 37;

	public static int str_to_the_east = 38;

	public static int str_to_the_southeast = 39;

	public static int str_to_the_south = 40;

	public static int str_to_the_southwest = 41;

	public static int str_to_the_west = 42;

	public static int str_to_the_northwest = 43;

	public static int str_far_far_below_you_1 = 44;

	public static int str_far_far_below_you_2 = 45;

	public static int str_far_below_you_1 = 46;

	public static int str_far_below_you_2 = 47;

	public static int str_below_you_1 = 48;

	public static int str_below_you_2 = 49;

	public static int str_underneath_you = 50;

	public static int str_above_you_1 = 52;

	public static int str_above_you_2 = 53;

	public static int str_above_you_3 = 54;

	public static int str_far_above_you_1 = 55;

	public static int str_far_above_you_2 = 56;

	public static int str_far_far_above_you_1 = 57;

	public static int str_far_far_above_you_2 = 58;

	public static int str_you_detect_a_creature_ = 59;

	public static int str_you_detect_a_few_creatures_ = 60;

	public static int str_you_detect_the_activity_of_many_creatures_ = 61;

	public static int str_you_detect_no_monster_activity_ = 62;

	public static int str__is_currently_active_ = 63;

	public static int str_you_are_currently_ = 64;

	public static int str_you_are_on_the_ = 65;

	public static int str__level_of_the_abyss_ = 66;

	public static int str_it_is_the_ = 67;

	public static int str__day_of_your_imprisonment_ = 68;

	public static int str_it_has_been_an_uncountable_number_of_days_since_you_entered_the_abyss_ = 69;

	public static int str_you_guess_that_it_is_currently_ = 70;

	public static int str_night_1 = 71;

	public static int str_night_2 = 72;

	public static int str_dawn = 73;

	public static int str_early_morning = 74;

	public static int str_morning = 75;

	public static int str_late_morning = 76;

	public static int str_mid_day = 77;

	public static int str_afternoon = 78;

	public static int str_late_afternoon = 79;

	public static int str_early_evening = 80;

	public static int str_evening = 81;

	public static int str_night_3 = 82;

	public static int str__ = 83;

	public static int str_barely = 84;

	public static int str_mildly = 85;

	public static int str_badly = 86;

	public static int str_seriously = 87;

	public static int str_egregiously = 88;

	public static int str_your_current_vitality_is_ = 89;

	public static int str_your_current_mana_points_are_ = 90;

	public static int str_you_are_ = 91;

	public static int str__poisoned_ = 92;

	public static int str_that_is_too_far_away_to_take_ = 93;

	public static int str_you_cannot_reach_that_ = 94;

	public static int str_that_is_too_heavy_for_you_to_pick_up_ = 95;

	public static int str_you_cannot_pick_that_up_ = 96;

	public static int str_you_detect_no_traps_ = 97;

	public static int str_you_found_a_trap__do_you_wish_to_try_to_disarm_it_1 = 98;

	public static int str_you_catch_a_lovely_fish_ = 99;

	public static int str_no_luck_this_time_ = 100;

	public static int str_you_cannot_fish_there__perhaps_somewhere_else_ = 101;

	public static int str_you_feel_a_nibble_but_the_fish_gets_away_ = 102;

	public static int str__and_ = 103;

	public static int str_starving = 104;

	public static int str_famished = 105;

	public static int str_very_hungry = 106;

	public static int str_hungry = 107;

	public static int str_peckish = 108;

	public static int str_fed = 109;

	public static int str_well_fed = 110;

	public static int str_full = 111;

	public static int str_satiated = 112;

	public static int str_fatigued = 113;

	public static int str_very_tired = 114;

	public static int str_drowsy = 115;

	public static int str_awake = 116;

	public static int str_rested = 117;

	public static int str_wide_awake = 118;

	public static int str_you_can_only_use_those_individually__take_one_from_this_group_if_you_wish_to_use_it_ = 119;

	public static int str_your_lockpicking_attempt_failed_ = 120;

	public static int str_you_succeed_in_picking_the_lock_ = 121;

	public static int str_that_is_not_locked_ = 122;

	public static int str_lights_may_only_be_used_if_equipped_ = 123;

	public static int str_that_light_is_already_used_up_ = 124;

	public static int str_with_a_loud_snap_the_wand_cracks_ = 125;

	public static int str_you_are_too_full_to_eat_that_now_ = 126;

	public static int str_it_seems_to_have_no_effect_ = 132;

	public static int str_you_thoughtfully_give_the_bones_a_final_resting_place_ = 134;

	public static int str_the_rock_breaks_into_smaller_pieces_ = 135;

	public static int str__is_nearly_done_ = 137;

	public static int str__is_unstable_ = 138;

	public static int str__is_stable_ = 139;

	public static int str_you_destroyed_the_ = 140;

	public static int str_you_damaged_the_ = 141;

	public static int str_you_cannot_repair_that_ = 142;

	public static int str_your_attempt_has_no_effect_on_the_ = 143;

	public static int str_you_have_partially_repaired_the_ = 144;

	public static int str_you_have_fully_repaired_the_ = 145;

	public static int str_the_noise_you_made_has_attracted_attention_ = 146;

	public static int str_you_have_attained_experience_level_ = 147;

	public static int str_the_bowl_does_not_contain_the_correct_ingredients_ = 148;

	public static int str_you_mix_the_ingredients_into_a_stew_ = 149;

	public static int str_you_need_a_bowl_to_mix_the_ingredients_ = 150;

	public static int str_enscribed_upon_the_scroll_is_your_map_ = 151;

	public static int str_you_cannot_use_that_ = 152;

	public static int str_why_is_this_being_printed_ = 153;

	public static int str_you_see_nothing_ = 154;

	public static int str_nothing_to_get_ = 155;

	public static int str_you_cannot_talk_to_that_ = 156;

	public static int str_using_the_pole_you_trigger_the_switch_ = 157;

	public static int str_the_pole_cannot_be_used_on_that_ = 158;

	public static int str_impossible_you_are_between_worlds_ = 159;

	public static int str_you_cannot_select_options_partway_through_an_action_ = 160;

	public static int str_no_save_game_there_ = 161;

	public static int str_restore_game_complete_ = 162;

	public static int str_restore_game_failed_ = 163;

	public static int str_save_game_failed_ = 164;

	public static int str_save_game_succeeded_ = 165;

	public static int str_restoring_game_ = 166;

	public static int str_saving_game_ = 167;

	public static int str_please_enter_a_save_file_description_ = 168;

	public static int str_error_bad_save_file = 169;

	public static int str_you_see_a_bridge_ = 171;

	public static int str__tasted_putrid_ = 172;

	public static int str__tasted_a_little_rancid_ = 173;

	public static int str__tasted_kind_of_bland_ = 174;

	public static int str__tasted_pretty_good_ = 175;

	public static int str__tasted_great_ = 176;

	public static int str_you_cannot_use_oil_on_that_ = 177;

	public static int str_you_think_it_is_a_bad_idea_to_add_oil_to_the_lit_lantern_ = 178;

	public static int str_adding_oil_you_refuel_the_lantern_ = 179;

	public static int str_the_lantern_is_already_full_ = 180;

	public static int str_dousing_a_cloth_with_oil_and_applying_it_to_the_wood_you_make_a_torch_ = 181;

	public static int str_you_think_it_is_a_bad_idea_to_add_oil_to_the_lit_torch_ = 182;

	public static int str_you_refresh_the_torch_ = 183;

	public static int str_the_torch_is_unused_ = 184;

	public static int str_you_are_unable_to_use_that_from_here_ = 185;

	public static int str_you_cannot_barter_a_container__instead_remove_the_contents_you_want_to_trade_ = 186;

	public static int str_a_level_ = 187;

	public static int str_after_ = 189;

	public static int str_you_press_in_the_button_1 = 194;

	public static int str_you_press_in_the_button_2 = 195;

	public static int str_you_press_in_the_button_3 = 196;

	public static int str_you_flip_the_switch_1 = 197;

	public static int str_you_flip_the_switch_2 = 198;

	public static int str_you_pull_the_lever_1 = 199;

	public static int str_you_pull_the_chain_1 = 200;

	public static int str_you_pull_the_chain_2 = 201;

	public static int str_the_button_returns_to_its_original_state_1 = 202;

	public static int str_the_button_returns_to_its_original_state_2 = 203;

	public static int str_the_button_returns_to_its_original_state_3 = 204;

	public static int str_you_flip_the_switch_3 = 205;

	public static int str_you_flip_the_switch_4 = 206;

	public static int str_you_push_the_lever_2 = 207;

	public static int str_you_return_the_chain_to_its_original_position_1 = 208;

	public static int str_you_return_the_chain_to_its_original_position_2 = 209;

	public static int str_you_are_not_experienced_enough_to_cast_spells_of_that_circle_ = 210;

	public static int str_you_do_not_have_enough_mana_to_cast_the_spell_ = 211;

	public static int str_the_incantation_failed_ = 212;

	public static int str_casting_was_not_successful_ = 213;

	public static int str_the_spell_backfires_ = 214;

	public static int str_you_attempt_to_use_the_wand_ = 215;

	public static int str_you_think_it_will_be_ = 216;

	public static int str__to_repair_the_ = 217;

	public static int str__make_an_attempt_ = 218;

	public static int str_trivial = 219;

	public static int str_simple = 220;

	public static int str_possible = 221;

	public static int str_hard = 222;

	public static int str_very_difficult = 223;

	public static int str_the_leeches_remove_the_poison_as_well_as_some_of_your_skin_and_blood_ = 224;

	public static int str__is_angered_by_your_action_ = 225;

	public static int str__is_annoyed_by_your_action_ = 226;

	public static int str__notes_your_action_ = 227;

	public static int str_your_vision_distorts_and_you_feel_light_headed_ = 228;

	public static int str_you_manage_to_finish_eating_the_leeches__barely_ = 229;

	public static int str_you_eat_the_candle_but_doubt_that_it_was_good_for_you_ = 230;

	public static int str_the_toadstool_tastes_odd_and_you_begin_to_feel_ill_ = 231;

	public static int str_the_mushroom_causes_your_head_to_spin_and_your_vision_to_blur_ = 232;

	public static int str_the_plant_is_plain_tasting_but_nourishing_ = 233;

	public static int str_although_you_have_to_eat_around_the_thorny_flowers_the_plant_is_quite_good_ = 236;

	public static int str_the_water_refreshes_you_ = 237;

	public static int str_you_drink_the_dark_ale_ = 239;

	public static int str_you_quaff_the_potion_in_one_gulp_ = 240;

	public static int str_as_the_alcohol_hits_you_you_stumble_and_collapse_into_sleep_ = 241;

	public static int str_the_drink_makes_you_feel_a_little_better_for_now_ = 242;

	public static int str_you_wake_feeling_somewhat_unstable_but_better_ = 243;

	public static int str_you_found_a_trap__do_you_wish_to_try_to_disarm_it_2 = 244;

	public static int str_your_rune_of_warding_has_been_set_off_ = 245;

	public static int str_your_hands_are_full_ = 246;

	public static int str_you_can_only_put_runes_in_the_rune_bag_ = 247;

	public static int str_that_item_does_not_fit_ = 248;

	public static int str_the_waters_of_the_fountain_renew_your_strength_ = 249;

	public static int str_you_play_the_instrument_ = 250;

	public static int str_you_put_the_instrument_down_ = 251;

	public static int str_that_is_too_heavy_to_take_ = 252;

	public static int str_there_is_no_space_to_drop_that_ = 253;

	public static int str_you_need_more_space_to_fire_that_weapon_ = 254;

	public static int str_there_is_not_enough_room_to_release_that_spell_ = 255;

	public static int str_enters_the_abyss___ = 256;

	public static int str_you_reenter_the_abyss___ = 257;

	public static int str_there_is_no_place_to_put_that_ = 258;

	public static int str_you_see_ = 260;

	public static int str_the_spell_unlocks_the_lock_ = 270;

	public static int str_the_spell_has_no_discernable_effect_ = 271;

	public static int str_the_moonstone_is_not_available_ = 273;

	public static int str_the_cauldron_is_empty_ = 274;

	public static int str_gamename = 275;

	public static int str_there_is_no_room_to_create_that_ = 277;

	public static int str_a_rending_sound_fills_the_air_ = 278;

	public static int str_you_cannot_save_or_restore_games_in_the_demo_version_ = 281;

	private Hashtable GameStrings;

	private Hashtable EntryCounts;

	public static StringController instance;

	private void Awake()
	{
		instance = this;
	}

	public bool InitStringController(string StringFilePath)
	{
		GameStrings = new Hashtable();
		EntryCounts = new Hashtable();
		return Load(StringFilePath);
	}

	public void LoadStringsPak(string path)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			str_it_looks_to_be_that_of_ = 26;
			str_they_look_to_be_those_of_ = 27;
			str_you_are_not_ready_to_advance_ = 28;
			str_that_is_not_a_mantra_ = 29;
			str_knowledge_and_understanding_fill_you_ = 30;
			str_you_cannot_advance_any_further_in_that_skill_ = 31;
			str_you_have_advanced_greatly_in_ = 32;
			str_you_have_advanced_in_ = 33;
			str_none_of_your_skills_improved_ = 34;
			str_you_cannot_advance_in_ = 38;
			str_to_the_north = 40;
			str_to_the_northeast = 41;
			str_to_the_east = 42;
			str_to_the_southeast = 43;
			str_to_the_south = 44;
			str_to_the_southwest = 45;
			str_to_the_west = 46;
			str_to_the_northwest = 47;
			str_far_far_below_you_1 = 48;
			str_far_far_below_you_2 = 48;
			str_far_below_you_1 = 50;
			str_far_below_you_2 = 50;
			str_below_you_1 = 52;
			str_below_you_2 = 52;
			str_underneath_you = 54;
			str_above_you_1 = 56;
			str_above_you_2 = 56;
			str_above_you_3 = 56;
			str_far_above_you_1 = 59;
			str_far_above_you_2 = 59;
			str_far_far_above_you_1 = 61;
			str_far_far_above_you_2 = 61;
			str_you_detect_a_creature_ = 63;
			str_you_detect_a_few_creatures_ = 64;
			str_you_detect_the_activity_of_many_creatures_ = 65;
			str_you_detect_no_monster_activity_ = 66;
			str__is_currently_active_ = 67;
			str_you_are_currently_ = 68;
			str_you_are_on_the_ = 73;
			str__level_of_the_abyss_ = 74;
			str_it_is_the_ = 69;
			str__day_of_your_imprisonment_ = 70;
			str_it_has_been_an_uncountable_number_of_days_since_you_entered_the_abyss_ = 71;
			str_you_guess_that_it_is_currently_ = 72;
			str_night_1 = 84;
			str_night_2 = 84;
			str_dawn = 86;
			str_early_morning = 87;
			str_morning = 88;
			str_late_morning = 89;
			str_mid_day = 90;
			str_afternoon = 91;
			str_late_afternoon = 92;
			str_early_evening = 93;
			str_evening = 94;
			str_night_3 = 84;
			str__ = 96;
			str_barely = 97;
			str_mildly = 98;
			str_badly = 99;
			str_seriously = 100;
			str_egregiously = 101;
			str_your_current_vitality_is_ = 102;
			str_your_current_mana_points_are_ = 103;
			str_you_are_ = 104;
			str__poisoned_ = 105;
			str_that_is_too_far_away_to_take_ = 106;
			str_you_cannot_reach_that_ = 107;
			str_that_is_too_heavy_for_you_to_pick_up_ = 108;
			str_you_cannot_pick_that_up_ = 109;
			str_you_detect_no_traps_ = 110;
			str_you_found_a_trap__do_you_wish_to_try_to_disarm_it_1 = 111;
			str_you_catch_a_lovely_fish_ = 112;
			str_no_luck_this_time_ = 113;
			str_you_cannot_fish_there__perhaps_somewhere_else_ = 114;
			str_you_feel_a_nibble_but_the_fish_gets_away_ = 115;
			str__and_ = 116;
			str_starving = 117;
			str_famished = 118;
			str_very_hungry = 119;
			str_hungry = 120;
			str_peckish = 121;
			str_fed = 122;
			str_well_fed = 123;
			str_full = 124;
			str_satiated = 125;
			str_fatigued = 126;
			str_very_tired = 127;
			str_drowsy = 128;
			str_awake = 129;
			str_rested = 130;
			str_wide_awake = 131;
			str_you_can_only_use_those_individually__take_one_from_this_group_if_you_wish_to_use_it_ = 132;
			str_your_lockpicking_attempt_failed_ = 133;
			str_you_succeed_in_picking_the_lock_ = 135;
			str_that_is_not_locked_ = 136;
			str_lights_may_only_be_used_if_equipped_ = 137;
			str_that_light_is_already_used_up_ = 138;
			str_with_a_loud_snap_the_wand_cracks_ = 139;
			str_you_are_too_full_to_eat_that_now_ = 140;
			str_it_seems_to_have_no_effect_ = 146;
			str_you_thoughtfully_give_the_bones_a_final_resting_place_ = 148;
			str_the_rock_breaks_into_smaller_pieces_ = 149;
			str__is_nearly_done_ = 151;
			str__is_unstable_ = 152;
			str__is_stable_ = 153;
			str_you_destroyed_the_ = 154;
			str_you_damaged_the_ = 155;
			str_you_cannot_repair_that_ = 156;
			str_your_attempt_has_no_effect_on_the_ = 157;
			str_you_have_partially_repaired_the_ = 158;
			str_you_have_fully_repaired_the_ = 159;
			str_the_noise_you_made_has_attracted_attention_ = 160;
			str_you_have_attained_experience_level_ = 161;
			str_the_bowl_does_not_contain_the_correct_ingredients_ = 162;
			str_you_mix_the_ingredients_into_a_stew_ = 163;
			str_you_need_a_bowl_to_mix_the_ingredients_ = 164;
			str_enscribed_upon_the_scroll_is_your_map_ = 165;
			str_you_cannot_use_that_ = 166;
			str_why_is_this_being_printed_ = 167;
			str_you_see_nothing_ = 168;
			str_nothing_to_get_ = 169;
			str_you_cannot_talk_to_that_ = 170;
			str_using_the_pole_you_trigger_the_switch_ = 171;
			str_the_pole_cannot_be_used_on_that_ = 172;
			str_impossible_you_are_between_worlds_ = 173;
			str_you_cannot_select_options_partway_through_an_action_ = 174;
			str_no_save_game_there_ = 175;
			str_restore_game_complete_ = 176;
			str_restore_game_failed_ = 177;
			str_save_game_failed_ = 178;
			str_save_game_succeeded_ = 179;
			str_restoring_game_ = 181;
			str_saving_game_ = 182;
			str_please_enter_a_save_file_description_ = 183;
			str_error_bad_save_file = 184;
			str_you_see_a_bridge_ = 186;
			str__tasted_putrid_ = 187;
			str__tasted_a_little_rancid_ = 188;
			str__tasted_kind_of_bland_ = 189;
			str__tasted_pretty_good_ = 190;
			str__tasted_great_ = 191;
			str_you_cannot_use_oil_on_that_ = 192;
			str_you_think_it_is_a_bad_idea_to_add_oil_to_the_lit_lantern_ = 193;
			str_adding_oil_you_refuel_the_lantern_ = 194;
			str_the_lantern_is_already_full_ = 195;
			str_dousing_a_cloth_with_oil_and_applying_it_to_the_wood_you_make_a_torch_ = 196;
			str_you_think_it_is_a_bad_idea_to_add_oil_to_the_lit_torch_ = 197;
			str_you_refresh_the_torch_ = 198;
			str_the_torch_is_unused_ = 199;
			str_you_are_unable_to_use_that_from_here_ = 200;
			str_you_cannot_barter_a_container__instead_remove_the_contents_you_want_to_trade_ = 201;
			str_a_level_ = 202;
			str_after_ = 204;
			str_you_press_in_the_button_1 = 209;
			str_you_press_in_the_button_2 = 209;
			str_you_press_in_the_button_3 = 209;
			str_you_flip_the_switch_1 = 212;
			str_you_flip_the_switch_2 = 212;
			str_you_pull_the_lever_1 = 214;
			str_you_pull_the_chain_1 = 215;
			str_you_pull_the_chain_2 = 215;
			str_the_button_returns_to_its_original_state_1 = 217;
			str_the_button_returns_to_its_original_state_2 = 217;
			str_the_button_returns_to_its_original_state_3 = 217;
			str_you_flip_the_switch_1 = 212;
			str_you_flip_the_switch_2 = 212;
			str_you_push_the_lever_2 = 222;
			str_you_return_the_chain_to_its_original_position_1 = 223;
			str_you_return_the_chain_to_its_original_position_2 = 223;
			str_you_are_not_experienced_enough_to_cast_spells_of_that_circle_ = 225;
			str_you_do_not_have_enough_mana_to_cast_the_spell_ = 226;
			str_the_incantation_failed_ = 227;
			str_casting_was_not_successful_ = 228;
			str_the_spell_backfires_ = 229;
			str_you_attempt_to_use_the_wand_ = 230;
			str_you_think_it_will_be_ = 231;
			str__to_repair_the_ = 232;
			str__make_an_attempt_ = 233;
			str_trivial = 234;
			str_simple = 235;
			str_possible = 236;
			str_hard = 237;
			str_very_difficult = 238;
			str_the_leeches_remove_the_poison_as_well_as_some_of_your_skin_and_blood_ = 239;
			str__is_angered_by_your_action_ = 240;
			str__is_annoyed_by_your_action_ = 241;
			str__notes_your_action_ = 242;
			str_your_vision_distorts_and_you_feel_light_headed_ = 243;
			str_you_manage_to_finish_eating_the_leeches__barely_ = 244;
			str_you_eat_the_candle_but_doubt_that_it_was_good_for_you_ = 245;
			str_the_toadstool_tastes_odd_and_you_begin_to_feel_ill_ = 246;
			str_the_mushroom_causes_your_head_to_spin_and_your_vision_to_blur_ = 247;
			str_the_plant_is_plain_tasting_but_nourishing_ = 248;
			str_although_you_have_to_eat_around_the_thorny_flowers_the_plant_is_quite_good_ = 251;
			str_the_water_refreshes_you_ = 252;
			str_you_drink_the_dark_ale_ = 254;
			str_you_quaff_the_potion_in_one_gulp_ = 255;
			str_as_the_alcohol_hits_you_you_stumble_and_collapse_into_sleep_ = 256;
			str_the_drink_makes_you_feel_a_little_better_for_now_ = 257;
			str_you_wake_feeling_somewhat_unstable_but_better_ = 258;
			str_you_found_a_trap__do_you_wish_to_try_to_disarm_it_2 = 259;
			str_your_rune_of_warding_has_been_set_off_ = 260;
			str_your_hands_are_full_ = 261;
			str_you_can_only_put_runes_in_the_rune_bag_ = 262;
			str_that_item_does_not_fit_ = 263;
			str_the_waters_of_the_fountain_renew_your_strength_ = 264;
			str_you_play_the_instrument_ = 265;
			str_you_put_the_instrument_down_ = 266;
			str_that_is_too_heavy_to_take_ = 268;
			str_there_is_no_space_to_drop_that_ = 269;
			str_you_need_more_space_to_fire_that_weapon_ = 270;
			str_there_is_not_enough_room_to_release_that_spell_ = 271;
			str_enters_the_abyss___ = 272;
			str_you_reenter_the_abyss___ = 273;
			str_there_is_no_place_to_put_that_ = 274;
			str_you_see_ = 276;
			str_the_spell_unlocks_the_lock_ = 286;
			str_the_spell_has_no_discernable_effect_ = 287;
			str_the_moonstone_is_not_available_ = 289;
			str_the_cauldron_is_empty_ = 290;
			str_gamename = 291;
			str_there_is_no_room_to_create_that_ = 293;
			str_a_rending_sound_fills_the_air_ = 294;
			str_you_cannot_save_or_restore_games_in_the_demo_version_ = 297;
		}
		string text = "";
		long num = 0L;
		string text2 = "";
		GameStrings = new Hashtable();
		EntryCounts = new Hashtable();
		char[] buffer;
		if (!DataLoader.ReadStreamFile(path, out buffer))
		{
			return;
		}
		long valAtAddress = DataLoader.getValAtAddress(buffer, num, 16);
		int num2 = 0;
		huffman_node[] array = new huffman_node[valAtAddress];
		num += 2;
		while (num2 < valAtAddress)
		{
			array[num2].symbol = Convert.ToChar(buffer[num]);
			array[num2].parent = buffer[num + 1];
			array[num2].left = buffer[num + 2];
			array[num2].right = buffer[num + 3];
			num2++;
			num += 4;
		}
		long valAtAddress2 = DataLoader.getValAtAddress(buffer, num, 16);
		block_dir[] array2 = new block_dir[valAtAddress2];
		num += 2;
		for (num2 = 0; num2 < valAtAddress2; num2++)
		{
			array2[num2].block_no = DataLoader.getValAtAddress(buffer, num, 16);
			num += 2;
			array2[num2].address = DataLoader.getValAtAddress(buffer, num, 32);
			num += 4;
			array2[num2].NoOfEntries = DataLoader.getValAtAddress(buffer, array2[num2].address, 16);
			EntryCounts["_" + array2[num2].block_no] = array2[num2].NoOfEntries.ToString();
		}
		num2 = 0;
		int num3 = 0;
		for (; num2 < valAtAddress2; num2++)
		{
			num = 2 + array2[num2].address + array2[num2].NoOfEntries * 2;
			for (int i = 0; i < array2[num2].NoOfEntries; i++)
			{
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				long num7 = 0L;
				do
				{
					num7 = valAtAddress - 1;
					while (array[num7].left != 255 && array[num7].right != 255)
					{
						if (num5 == 0)
						{
							num5 = 8;
							num6 = buffer[num++];
						}
						num3++;
						num7 = (((num6 & 0x80) != 128) ? array[num7].left : array[num7].right);
						num6 <<= 1;
						num5--;
					}
					if (array[num7].symbol != '|')
					{
						if (num4 == 0)
						{
							text2 = array2[num2].block_no.ToString("000") + "_" + i.ToString("000");
						}
						text += array[num7].symbol;
						num4 = 1;
					}
					else if (text.Length > 0 && text2.Length > 0)
					{
						GameStrings[text2] = text;
						text = "";
						text2 = "";
					}
				}
				while (array[num7].symbol != '|');
			}
		}
		if (text.Length > 0 && text2.Length > 0)
		{
			GameStrings[text2] = text;
			text = "";
		}
	}

	public string GetString(int BlockNo, int StringNo)
	{
		if (GameStrings == null)
		{
			return "";
		}
		string text = (string)GameStrings[BlockNo.ToString("000") + "_" + StringNo.ToString("000")];
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public string GetObjectNounUW(ObjectInteraction objInt)
	{
		return GetObjectNounUW(objInt.item_id);
	}

	public string GetObjectNounUW(int item_id)
	{
		string text = GetString(4, item_id);
		if (text.Contains("&"))
		{
			text = text.Split('&')[0];
		}
		text = text.Replace("a_", "");
		text = text.Replace("an_", "");
		return text.Replace("some_", "");
	}

	public string GetFormattedObjectNameUW(ObjectInteraction objInt)
	{
		string text = GetString(4, objInt.item_id);
		if (objInt.isQuant && !objInt.isEnchanted)
		{
			if (text.Contains("&"))
			{
				text = ((objInt.link <= 1) ? text.Split('&')[0] : (objInt.link + " " + text.Split('&')[1]));
			}
			else if (objInt.link > 1)
			{
				text = text.Replace("a_", objInt.link + "_");
				text = text.Replace("an_", objInt.link + "_");
				text = text.Replace("some_", objInt.link + "_");
				text += "s";
			}
		}
		else if (text.Contains("&"))
		{
			text = text.Split('&')[0];
		}
		text = text.Replace("_", " ");
		return GetString(1, str_you_see_) + text;
	}

	public string GetFormattedObjectNameUW(ObjectInteraction objInt, int Quantity)
	{
		string text = GetString(4, objInt.item_id);
		if (objInt.isQuant && !objInt.isEnchanted)
		{
			if (text.Contains("&"))
			{
				text = ((objInt.link <= 1 || Quantity <= 1) ? text.Split('&')[0] : (objInt.link + " " + text.Split('&')[1]));
			}
			else
			{
				text = text.Replace("a_", Quantity + "_");
				text = text.Replace("an_", Quantity + "_");
				text = text.Replace("some_", Quantity + "_");
			}
		}
		else if (text.Contains("&"))
		{
			text = text.Split('&')[0];
		}
		text = text.Replace("_", " ");
		return GetString(1, str_you_see_) + text;
	}

	public string GetFormattedObjectNameUW(ObjectInteraction objInt, string QualityString)
	{
		string text = GetString(4, objInt.item_id);
		if (text == null)
		{
			text = "";
		}
		if (objInt.isQuant && text.Contains("&") && !objInt.isEnchanted)
		{
			text = ((objInt.link <= 1) ? text.Split('&')[0] : (objInt.link + " " + text.Split('&')[1]));
		}
		else if (text.Contains("&"))
		{
			text = text.Split('&')[0];
		}
		switch (QualityString.Substring(0, 1).ToUpper())
		{
		case "A":
		case "E":
		case "I":
		case "O":
		case "U":
			text = ((!text.Contains("a_")) ? ((!text.Contains("_")) ? (QualityString + " " + text + " ") : text.Replace("_", " " + QualityString + " ")) : text.Replace("a_", "an " + QualityString + " "));
			break;
		default:
			text = ((!text.Contains("an_")) ? ((!text.Contains("_")) ? (QualityString + " " + text + " ") : text.Replace("_", " " + QualityString + " ")) : text.Replace("an_", "a " + QualityString + " "));
			break;
		}
		return GetString(1, str_you_see_) + text;
	}

	public string GetSimpleObjectNameUW(int item_id)
	{
		string text = GetString(4, item_id);
		if (text == null)
		{
			return "";
		}
		if (text.Contains("&"))
		{
			text = text.Split('&')[0];
		}
		return text.Replace("_", " ");
	}

	public string GetSimpleObjectNameUW(ObjectInteraction objInt)
	{
		return GetSimpleObjectNameUW(objInt.item_id);
	}

	public string TextureDescription(int index)
	{
		return GetString(1, str_you_see_) + GetString(10, index);
	}

	private bool Load(string fileName)
	{
		StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
		string key = "";
		string text = "";
		using (streamReader)
		{
			string text2;
			do
			{
				text2 = streamReader.ReadLine();
				if (text2 == null)
				{
					continue;
				}
				if (text2.IndexOf("~") >= 0)
				{
					string[] array = text2.Split('~');
					if (array.Length > 0)
					{
						GameStrings[array[1] + "_" + array[2]] = array[3];
						text = array[3];
						key = array[1] + "_" + array[2];
					}
				}
				else
				{
					GameStrings[key] = text + "\n" + text2;
					text = text + "\n" + text2;
				}
			}
			while (text2 != null);
			streamReader.Close();
			return true;
		}
	}

	public string GetTextureName(int index)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			return GetString(10, index);
		}
		if (index < 210)
		{
			return GetString(10, index);
		}
		return GetString(10, 510 - index + 210);
	}

	public int AddString(int BlockNo, string NewString)
	{
		string s = (string)EntryCounts["_" + BlockNo];
		int num = int.Parse(s);
		num++;
		GameStrings[BlockNo.ToString("000") + "_" + num.ToString("000")] = NewString;
		EntryCounts["_" + BlockNo] = num.ToString();
		return num;
	}
}
