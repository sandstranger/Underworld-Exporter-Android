package com.directorypicker;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.Environment;

import com.obsez.android.lib.filechooser.ChooserDialog;
import com.unity3d.player.UnityPlayer;

import java.io.File;

public class DirectoryPickerActivity extends Activity {
    private static final int CHOOSE_DIRECTORY_REQUEST_CODE = 292;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        boolean isTelevision =this.getPackageManager().hasSystemFeature(PackageManager.FEATURE_LEANBACK);
        if (isTelevision){
            new ChooserDialog(this)
                    .withFilter(true, false)
                    .withStartFile(Environment.getExternalStorageDirectory().getAbsolutePath())
                    // to handle the result(s)
                    .withChosenListener((directoryPath, file) -> {
                        sendMessageToUnity(directoryPath);
                        DirectoryPickerActivity.this.finish();
                    })
                    .build()
                    .show();
        }
        else {
            Intent intent = new Intent(Intent.ACTION_OPEN_DOCUMENT_TREE);
            intent.addCategory(Intent.CATEGORY_DEFAULT);
            startActivityForResult(Intent.createChooser(intent, "Choose directory"),
                    CHOOSE_DIRECTORY_REQUEST_CODE);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if ( requestCode == CHOOSE_DIRECTORY_REQUEST_CODE  && resultCode == RESULT_OK && data!=null){
            String directoryPath = Utils.Companion.getDirectoryPath(data);
            sendMessageToUnity(directoryPath);
        }

        this.finish();
    }

    private void sendMessageToUnity (String directoryPath){
        UnityPlayer.UnitySendMessage("Canvas", "SetGamePath",directoryPath);
    }
}

