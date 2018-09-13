
#include "MinhaInfo.h"


MinhaInfo::MinhaInfo(int vlr) {
	this->valor = vlr;
}
ostream& MinhaInfo::print(ostream& os)const throw()  {
	return os << this->valor;
}
int MinhaInfo::getValor()const throw() {
	return this->valor;
}
char MinhaInfo::operator<(const InfoArvoreEnaria& outra)const throw(){
	return this->valor <((const MinhaInfo&) outra).getValor();
}
char MinhaInfo::operator>(const InfoArvoreEnaria& outra)const throw() {
	return this->valor > ((const MinhaInfo&)outra).getValor();
}
char MinhaInfo::operator==(const InfoArvoreEnaria& outra)const throw() {
	return this->valor == ((const MinhaInfo&)outra).getValor();
}
char MinhaInfo::operator<=(const InfoArvoreEnaria& outra)const throw() {
	return this->valor <= ((const MinhaInfo&)outra).getValor();
}
char MinhaInfo::operator>=(const InfoArvoreEnaria& outra)const throw() {
	return this->valor >= ((const MinhaInfo&)outra).getValor();
}