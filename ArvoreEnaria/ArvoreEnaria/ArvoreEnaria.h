#pragma once

#ifndef ARVOREENARIA_H_INCLUDED
#define ARVOREENARIA_H_INCLUDED
#include "NoArvoreEnaria.h"
#include "InfoArvoreEnaria.h"
#include <iostream>
using namespace std;
class ArvoreEnaria
{
public:
	ArvoreEnaria(unsigned int);
	~ArvoreEnaria();
	char inserir(InfoArvoreEnaria*) throw(char*);
	char remover(InfoArvoreEnaria*) throw(char*);
	//virtual ostream& print(ostream&, const ArvoreEnaria) = 0;
	friend ostream& operator<< (ostream&, const ArvoreEnaria&) throw();
	NoArvoreEnaria* getRaiz() const throw(char*);
	
private:
	NoArvoreEnaria * raiz;
	int numInfo;
	
};

#endif
