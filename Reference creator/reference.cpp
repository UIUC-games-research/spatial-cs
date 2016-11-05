#include<fstream>
#include<iostream>
using std::fstream;
using std::string;
using std::ifstream;

string readFile(string address){
    ifstream fs(address);
    string content;
    string word;
    if (fs.is_open()){
        while(getline(fs, word)){
            content += word + "\n";
        }
    }
    fs.close();
    return content;
}

void writeFile(string address, string word){
    fstream fs(address, fstream::out);
    fs << word;
    fs.close();
}

int main(){
    string address = "data.txt";
    //writeFile(address, "Hello world\n");
    //writeFile(address, "second line\n");
    string a = readFile(address);
    int i = 0;
    while (a.find(",") < 10000){
        a.replace(a.find(","),1, "\n" + std::to_string(i) + " ");
        i++;
        
    }
    writeFile(address, a);
    return 0;
}
