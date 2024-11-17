package com.directorypicker

import android.content.Intent
import android.os.Environment

class Utils {
    companion object{
        fun getDirectoryPath(data: Intent) : String {
            data.data?.also { uri ->
                val pattern = Regex("[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}")
                val storageDir = Environment.getExternalStorageDirectory()
                val storagePath = storageDir.absolutePath
                val modifiedStoragePath = "/storage"
                val pathSegment = uri.lastPathSegment
                val currentGamePath = if (pattern.containsMatchIn(pathSegment ?: "")) {
                    modifiedStoragePath + "/" + pathSegment?.replace(":", "/")
                } else {
                    storagePath + "/" + pathSegment?.replace("primary:", "")
                }
                return currentGamePath
            }

            return ""
        }
    }
}