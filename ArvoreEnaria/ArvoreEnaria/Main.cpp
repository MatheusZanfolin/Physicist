// ArvoreEnaria.cpp : define o ponto de entrada para o aplicativo do console.
//
#include <iostream>
#include <cstdlib>
#include "ArvoreEnaria.h"
#include "MinhaInfo.h"
using namespace std;
int main()
{
	char x;
	unsigned int n;
	cout << "Digite o número de informações n de cada nó da árvore. O número de ponteiros vale n+1. ...";
	cin >> n;
	if (n == 0)
		cout << "Número não pode ser 0";
	ArvoreEnaria arvore(n);
	MinhaInfo info (2);
	InfoArvoreEnaria* minhaInfo = &info;
	cout << arvore;
	arvore.inserir(minhaInfo);
	cout << arvore;
	cin >> x;
    return 0;
}

