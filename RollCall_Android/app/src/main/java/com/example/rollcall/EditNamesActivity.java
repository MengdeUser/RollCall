package com.example.rollcall;

import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.appcompat.app.AppCompatActivity;

import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.BufferedReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.util.Arrays;
import java.util.ArrayList;

public class EditNamesActivity extends AppCompatActivity {

    private EditText etNewName;
    private Button btnAddName, btnDeleteName, btnSaveNames, btnImportNames, btnExportNames;
    private ListView lvNames;
    private Spinner spNameLists;
    private ArrayList<String> nameList;
    private ArrayAdapter<String> adapter;
    private int selectedPosition = -1;

    private ActivityResultLauncher<String> importFileLauncher;
    private ActivityResultLauncher<String> exportFileLauncher;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit_names);

        etNewName = findViewById(R.id.etNewName);
        btnAddName = findViewById(R.id.btnAddName);
        btnDeleteName = findViewById(R.id.btnDeleteName);
        btnSaveNames = findViewById(R.id.btnSaveNames);
        btnImportNames = findViewById(R.id.btnImportNames);
        btnExportNames = findViewById(R.id.btnExportNames);
        lvNames = findViewById(R.id.lvNames);

        nameList = loadNames();
        adapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, nameList);
        lvNames.setAdapter(adapter);

        btnAddName.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String newName = etNewName.getText().toString().trim();
                if (!newName.isEmpty()) {
                    nameList.add(newName);
                    adapter.notifyDataSetChanged();
                    etNewName.setText("");
                } else {
                    Toast.makeText(EditNamesActivity.this, "请输入名字！", Toast.LENGTH_SHORT).show();
                }
            }
        });

        btnDeleteName.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (selectedPosition != -1) {
                    nameList.remove(selectedPosition);
                    adapter.notifyDataSetChanged();
                    selectedPosition = -1;
                } else {
                    Toast.makeText(EditNamesActivity.this, "请先选择一个名字！", Toast.LENGTH_SHORT).show();
                }
            }
        });

        btnSaveNames.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveNames(nameList);
                Intent resultIntent = new Intent();
                resultIntent.putStringArrayListExtra("nameList", nameList);
                setResult(RESULT_OK, resultIntent);
                finish();
            }
        });

        btnImportNames.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                importFileLauncher.launch("text/plain");
            }
        });

        btnExportNames.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                exportFileLauncher.launch("名单.txt");
            }
        });

        lvNames.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                selectedPosition = position;
            }
        });

        importFileLauncher = registerForActivityResult(new ActivityResultContracts.GetContent(),
                uri -> {
                    if (uri != null) {
                        loadNamesFromFile(uri);
                    }
                });

        exportFileLauncher = registerForActivityResult(new ActivityResultContracts.CreateDocument("text/plain"),
                uri -> {
                    if (uri != null) {
                        saveNamesToFile(uri, nameList);
                    }
                });
    }

    private ArrayList<String> loadNames() {
        SharedPreferences prefs = getSharedPreferences("RollCallPrefs", MODE_PRIVATE);
        String names = prefs.getString("nameList", "");
        if (!names.isEmpty()) {
            return new ArrayList<>(Arrays.asList(names.split(";")));
        }
        return new ArrayList<>();
    }

    private void saveNames(ArrayList<String> names) {
        SharedPreferences prefs = getSharedPreferences("RollCallPrefs", MODE_PRIVATE);
        SharedPreferences.Editor editor = prefs.edit();
        String namesString = String.join(";", names);
        editor.putString("nameList", namesString);
        editor.apply();
    }

    private void loadNamesFromFile(Uri uri) {
        try {
            InputStream inputStream = getContentResolver().openInputStream(uri);
            BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
            String line;
            nameList.clear();
            while ((line = reader.readLine()) != null) {
                nameList.add(line.trim());
            }
            reader.close();
            adapter.notifyDataSetChanged();
            saveNames(nameList);
            Toast.makeText(this, "名单已成功导入！", Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Toast.makeText(this, "导入失败：" + e.getMessage(), Toast.LENGTH_SHORT).show();
        }
    }

    private void saveNamesToFile(Uri uri, ArrayList<String> names) {
        try {
            OutputStream outputStream = getContentResolver().openOutputStream(uri);
            OutputStreamWriter writer = new OutputStreamWriter(outputStream);
            for (String name : names) {
                writer.write(name + "\n");
            }
            writer.flush();
            writer.close();
            Toast.makeText(this, "名单已成功导出到：" + uri.getPath(), Toast.LENGTH_LONG).show();
        } catch (Exception e) {
            Toast.makeText(this, "导出失败：" + e.getMessage(), Toast.LENGTH_SHORT).show();
        }
    }
}
