package com.example.rollcall;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Typeface;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;

public class MainActivity extends AppCompatActivity {

    private ArrayList<String> nameList = new ArrayList<>();
    private ArrayList<String> historyList = new ArrayList<>();
    private CustomTextView resultTextView;
    private Button pickNameButton, editNamesButton, viewHistoryButton, endRoundButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        resultTextView = findViewById(R.id.tvSelectedName);
        pickNameButton = findViewById(R.id.btnPickName);
        endRoundButton = findViewById(R.id.btnEndRound); // 添加结束轮次按钮
        editNamesButton = findViewById(R.id.btnEditNames);
        viewHistoryButton = findViewById(R.id.btnViewHistory);

        // 加载自定义字体
        Typeface customFont = Typeface.createFromAsset(getAssets(), "fonts/HarmonyOS_Sans_SC_Medium.ttf");
        resultTextView.setTypeface(customFont);

        loadNames();

        pickNameButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (nameList.size() > 0) {
                    pickName();
                } else {
                    Toast.makeText(MainActivity.this, "当前轮次已结束，可以开始下一轮！", Toast.LENGTH_SHORT).show();
                    resetRound();
                }
            }
        });

        // 添加结束轮次按钮的点击事件
        endRoundButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                resetRound();
                Toast.makeText(MainActivity.this, "当前轮次已结束，可以开始下一轮！", Toast.LENGTH_SHORT).show();
            }
        });

        editNamesButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, EditNamesActivity.class);
                startActivityForResult(intent, 1);
            }
        });

        viewHistoryButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, HistoryActivity.class);
                intent.putStringArrayListExtra("historyList", historyList);
                startActivity(intent);
            }
        });
    }

    private void loadNames() {
        SharedPreferences prefs = getSharedPreferences("RollCallPrefs", MODE_PRIVATE);
        String names = prefs.getString("nameList", "");
        if (!names.isEmpty()) {
            nameList = new ArrayList<>(Arrays.asList(names.split(";")));
        }
    }

    private void pickName() {
        Random random = new Random();
        int index = random.nextInt(nameList.size());
        String name = nameList.get(index);
        nameList.remove(index); // 移除已点到的名字
        resultTextView.setText(name);
        historyList.add(name);
    }

    private void resetRound() {
        loadNames(); // 重新加载名单
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 1 && resultCode == RESULT_OK) {
            loadNames(); // 重新加载名单
            Toast.makeText(MainActivity.this, "名单已更新！", Toast.LENGTH_SHORT).show();
        }
    }
}
