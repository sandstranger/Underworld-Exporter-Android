package com.directorypicker;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import com.unity3d.player.UnityPlayer;

public class DirectoryPickerActivity extends Activity {
    private static final int CHOOSE_DIRECTORY_REQUEST_CODE = 292;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Intent intent = new Intent(Intent.ACTION_OPEN_DOCUMENT_TREE);
        intent.addCategory(Intent.CATEGORY_DEFAULT);
        startActivityForResult(Intent.createChooser(intent, "Choose directory"),
                CHOOSE_DIRECTORY_REQUEST_CODE);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if ( requestCode == CHOOSE_DIRECTORY_REQUEST_CODE  && resultCode == RESULT_OK && data!=null){
            String directoryPath = Utils.Companion.getDirectoryPath(data);
            UnityPlayer.UnitySendMessage("Canvas", "SetGamePath",directoryPath);
        }

        this.finish();
    }
}

