package com.logger;

import java.io.IOException;
import java.util.Arrays;
import java.util.List;

public class ExceptionsLogger {
    private Process process;

    public void startLog(String pathToLog) {
        stopLog();
        try {
            process = CreateProcess(pathToLog);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }

    public void stopLog() {
        if (process != null) {
            process.destroy();
        }
    }

    private Process CreateProcess(String pathToLog) throws IOException {
        ProcessBuilder processBuilder = new ProcessBuilder();
        List<String> commandToExecute = Arrays.asList("/system/bin/sh", "-c", "logcat *:W -d -f " + pathToLog);
        processBuilder.command(commandToExecute);
        processBuilder.redirectErrorStream(true);
        return processBuilder.start();
    }
}
