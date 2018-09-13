
#include "NoArvoreEnaria.h"


NoArvoreEnaria::NoArvoreEnaria(unsigned int numInfos)
{
	if (numInfos == 0)
		throw("Número de Informações inválido!");

	this->numInfos = numInfos;
	this->vetPtrInfo = new InfoArvoreEnaria*[numInfos];
	this->vetPtrNo = new NoArvoreEnaria*[numInfos + 1];
	for (int i = 0; i < numInfos; i++) {
		*(this->vetPtrInfo + i) = nullptr;
		*(this->vetPtrNo + i) = nullptr;
	}
	*(this->vetPtrNo + numInfos) = nullptr;

}
NoArvoreEnaria::NoArvoreEnaria(const NoArvoreEnaria& noBase)throw(char*)
{
	if(noBase.getNumInfos()==0)
		throw("Número de Informações do Objeto Base inválido!");
	this->numInfos = noBase.numInfos;
	this->vetPtrInfo = new InfoArvoreEnaria*[numInfos];
	this->vetPtrNo = new NoArvoreEnaria*[numInfos+1];
	int i;
	for (i = 0; i < numInfos; i++) {
		**(this->vetPtrInfo + i) = *(noBase.getPtrInfo(i));
		**(this->vetPtrNo + i) = *(noBase.getPtrNoFilho(i));
	}
	**(this->vetPtrNo + numInfos) = *(noBase.getPtrNoFilho(i));

}

NoArvoreEnaria::~NoArvoreEnaria()
{
}
int NoArvoreEnaria::getNumInfos()const throw() {
	return this->numInfos;
}
ostream& operator<< (ostream& os, const NoArvoreEnaria& no) throw() {
	int indicePtr = 0;
	int indiceInfo = 0;

	for (indicePtr = 0; indicePtr < no.getNumInfos() ; indicePtr++) {


		if (no.getPtrNoFilho(indicePtr) != nullptr) {
			os <<'(' << *(no.getPtrNoFilho(indicePtr))<<')';
		}
		else {
			os << "(  ||  )";
		}
		if (no.getPtrInfo(indicePtr) != nullptr) {
			
			os << "  " << *(no.getPtrInfo(indicePtr)) << "  ";
		}
		else {
			os << "  **  ";
		}
	}
	if (no.getPtrNoFilho(no.getNumInfos()) != nullptr) {
		os << '(' << *(no.getPtrNoFilho(no.getNumInfos())) << ')';
	}
	else {
		os << "(  ||  )";
	}
	return os;
}
NoArvoreEnaria* NoArvoreEnaria::getPtrNoFilho(unsigned int indFilho) const throw() {

	return *(vetPtrNo + indFilho);
}
InfoArvoreEnaria* NoArvoreEnaria::getPtrInfo(unsigned int indInfo) const throw() {
	return *(vetPtrInfo + indInfo);
}
char NoArvoreEnaria::colocarVetorOrdem(InfoArvoreEnaria* info) throw() {
	if (info == nullptr)
		return -1;
	//supondo vetor está ordenado;
	for (int i = 0; i < this->numInfos; i++) {
		if (*(vetPtrInfo + i) != nullptr) {
			if (**(vetPtrInfo + i) > *info) {
				//tem que inserir no lugar de i
				for (int j = numInfos - 1; j >= i; j--) {
					*(this->vetPtrInfo + j + 1) = *(this->vetPtrInfo + j);
				}
				*(this->vetPtrInfo + i) = info;
			}
		}
		else {
			*(vetPtrInfo + i) = info;
			return 1;
		}
	}
}
char NoArvoreEnaria::removerVetorOrdem(InfoArvoreEnaria* info) throw() {
	if (info == nullptr)
		return -1;
	if (!haInfo(info))
		return -1;
	//ordenar vetor
	int indiceInfo = -1;
	//supondo vetor está ordenado
	for (int i = 0; i < this->numInfos; i++) {
		if (*(vetPtrInfo + i) != nullptr) {
			if (**(vetPtrInfo + i) == *info) {
				indiceInfo = i;
			}
		}
	}
	if (indiceInfo == -1) //não achou essa info nesse nó
		return -1;
	if (!this->isCheio()) {//se não está cheio, é folha
		*(this->vetPtrInfo + indiceInfo) = nullptr;
		if (indiceInfo == 0) {//remover a informação 0, o vetor vai ficar vazio
			
		}
		for (int i = indiceInfo + 1; i < this->numInfos; i++) {
			*(this->vetPtrInfo + i - 1) = *(this->vetPtrInfo + i);

		}//reestruturando o nó

		
		return 1;
	}
	else {//se não é folha
		InfoArvoreEnaria* infoSubstit =	this->acharInfoPorLugar(indiceInfo);
		*(this->vetPtrInfo + indiceInfo) = nullptr;
		int indiceInserir;
		for (indiceInserir = 0; indiceInserir < this->numInfos; indiceInserir++) {
			if ((*(*(vetPtrInfo + indiceInserir))) > *infoSubstit) {
				goto saiLoop;
			}
			
		}
saiLoop:
		if ((*(*(vetPtrInfo + numInfos -1))) < *infoSubstit) {
			//informação Substituta é a maior do vetor
			indiceInserir = this->numInfos - 1;
		}
		if (indiceInserir < indiceInfo) {
			//deslocamento para direita
			for (int i = indiceInfo; i > indiceInserir; i--) {
				*(vetPtrInfo + i) = *(vetPtrInfo + i - 1);
				*(vetPtrNo + i + 1) = *(vetPtrNo + i);
				
			}
			*(vetPtrNo + indiceInserir + 1) = *(vetPtrNo + indiceInserir);
			*(vetPtrInfo + indiceInserir) = infoSubstit;
		}
		if (indiceInserir == indiceInfo) {
			*(this->vetPtrInfo + indiceInfo) = infoSubstit;
		}
		if (indiceInserir > indiceInfo) {
			//deslocamento para esquerda
			for (int i = indiceInfo; i < indiceInfo; i++) {
				*(vetPtrInfo + i) = *(vetPtrInfo + i + 1);

				*(vetPtrNo + i ) = *(vetPtrNo + i+ 1);
			}
			*(vetPtrNo + indiceInserir) = *(vetPtrNo + indiceInserir + 1);
			*(vetPtrInfo + indiceInserir) = infoSubstit;
			
		}
		return 1;
	}

}
char NoArvoreEnaria::isCheio() const throw() {
	
	for (int i = 0; i < numInfos; i++)
		if (*(vetPtrInfo + i) == nullptr)
			return 0;
	return 1;
}
char NoArvoreEnaria::isVazio() const throw() {
	
	for (int i = 0; i < numInfos; i++)
		if (*(vetPtrInfo + i) != nullptr)
			return 0;
	return 1;
}
char NoArvoreEnaria::haInfo(InfoArvoreEnaria* info)const throw() {
	NoArvoreEnaria* noRel = (NoArvoreEnaria*)(this);
	while (1) {
		for (int i = 0; i < numInfos; i++) {
			if (*(noRel->vetPtrInfo + i) != nullptr) {
				if (**(vetPtrInfo + i) == *info) {
					return true;
				}
				if (**(vetPtrInfo + i) > *info) {
					//ir pro ponteiro de nó i-1
					noRel = *(vetPtrNo + i - 1);

				}
				if (**(vetPtrInfo + i) < *info) {
					noRel = *(vetPtrNo + i + 1);
				}

			}
			else {
				return false;
			}
		}
	}
}
InfoArvoreEnaria* NoArvoreEnaria::acharInfoPorLugar(unsigned int indiceInfoTrocar) throw(char*) {
	InfoArvoreEnaria* infoATrocar = *(this->vetPtrInfo + indiceInfoTrocar);
	NoArvoreEnaria* noRel = (NoArvoreEnaria*)this;
	int i = 0;
	char sentido = 0;
	for(;i<noRel->numInfos;i++){
		if (sentido > -1 && *(noRel->vetPtrNo + indiceInfoTrocar+ i) != nullptr) {
			//achar o menor valor dessa subárvore direita
			int indiceInfoFilho = 0;
			/*while ((*(noRel->vetPtrNo + i))->getPtrInfo(indiceInfoFilho + 1) != nullptr)
				indiceInfoFilho++;*/
			//indiceInfoFilho representa o índice do menor info do filho encontrado
			if ((*(noRel->vetPtrNo + indiceInfoTrocar + i))->getPtrNoFilho(indiceInfoFilho) != nullptr) {
				//esse info tem outros infos antes dele
				sentido = 1;
				
				noRel = (*(noRel->vetPtrNo+ indiceInfoTrocar + i))->getPtrNoFilho(indiceInfoFilho);
				i = 0;
			}
			else {
				//esse info é o desejado para trocar
				
				InfoArvoreEnaria* infoTrocarFilho =
					((*(noRel->vetPtrNo+ indiceInfoTrocar + i))->getPtrInfo(indiceInfoFilho));
				(*(noRel->vetPtrNo+ indiceInfoTrocar + i))->removerVetorOrdem(infoTrocarFilho);
				if ((*(noRel->vetPtrNo + indiceInfoTrocar + i))->isVazio())
					(*(noRel->vetPtrNo + indiceInfoTrocar + i)) = nullptr;
				return infoTrocarFilho;
			}
			//(noRel->vetPtrNo + i)->removerVetorOrdem();
		}
		if (sentido <1 && *(noRel->vetPtrNo + indiceInfoTrocar- i) != nullptr) {
			//achar o maior valor dessa subárvore esquerda
			int indiceInfoFilho = -1;
			while ((*(noRel->vetPtrNo+ indiceInfoTrocar + i))->getPtrInfo(indiceInfoFilho + 1) != nullptr)
				indiceInfoFilho++;
			//indiceInfoFilho representa o índice do maior info do filho encontrado
			if ((*(noRel->vetPtrNo+ indiceInfoTrocar + i))->getPtrNoFilho(indiceInfoFilho + 1) != nullptr) {
				//esse info tem outros infos depois dele
				sentido = -1;
				
				noRel = (*(noRel->vetPtrNo+ indiceInfoTrocar + i))->getPtrNoFilho(indiceInfoFilho + 1);
				i = 0;
			}
			else {
				//esse info é o desejado para trocar
				
				InfoArvoreEnaria* infoTrocarFilho =
					((*(noRel->vetPtrNo+ indiceInfoTrocar + i))->getPtrInfo(indiceInfoFilho));
				(*(noRel->vetPtrNo+ indiceInfoTrocar + i))->removerVetorOrdem(infoTrocarFilho);
				if ((*(noRel->vetPtrNo + indiceInfoTrocar + i))->isVazio())
					(*(noRel->vetPtrNo + indiceInfoTrocar + i)) = nullptr;
				return infoTrocarFilho;
			}
		}
	}

};