import { Company } from './../../../common/domain/company/company';


export interface ShoreCompany extends Company {
    address: string;
    phone: string;
    email: string;
    manager: string;
    customerNumber: number;
}
