package com.directorypicker;

import android.content.Context;
import android.content.Intent;

public final class DirectoryPicker {
    public static void PickDirectory (Context context){
        context.startActivity(new Intent(context, DirectoryPickerActivity.class));
    }
}
