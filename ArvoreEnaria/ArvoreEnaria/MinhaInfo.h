#pragma once

#ifndef MINHAINFO_H_INCLUDED
#define MINHAINFO_H_INCLUDED
#include "InfoArvoreEnaria.h"
#include <iostream>
using namespace std;
class MinhaInfo : public InfoArvoreEnaria
{
public:
	MinhaInfo(int);
	friend ostream& operator<< (ostream&, const MinhaInfo&) throw();
	int getValor() const throw();
	char operator<(const InfoArvoreEnaria&)const throw() ;
	char operator>(const InfoArvoreEnaria&)const throw() ;
	char operator==(const InfoArvoreEnaria&)const throw();
	char operator<=(const InfoArvoreEnaria&)const throw();
	char operator>=(const InfoArvoreEnaria&)const throw();

	ostream& print(ostream&) const throw() ;
private:
	int valor;
};
#endif

