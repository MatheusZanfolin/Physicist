#pragma once
#ifndef INFOARVOREENARIA_H_INCLUDED
#define INFOARVOREENARIA_H_INCLUDED
#include <iostream>
using namespace std;
class InfoArvoreEnaria
{
public:
	InfoArvoreEnaria();
	~InfoArvoreEnaria();
	friend ostream& operator<< (ostream&, const InfoArvoreEnaria&) throw();
	virtual char operator<(const InfoArvoreEnaria&)const throw() = 0;
	virtual char operator>(const InfoArvoreEnaria&)const throw() = 0;
	virtual char operator==(const InfoArvoreEnaria&)const throw() = 0;
	virtual char operator<=(const InfoArvoreEnaria&)const throw() = 0;
	virtual char operator>=(const InfoArvoreEnaria&)const throw() = 0;

	virtual ostream& print(ostream&)const throw() = 0 ;

};
#endif
