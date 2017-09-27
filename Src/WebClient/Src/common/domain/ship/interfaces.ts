
export interface Ship {
    ShipModel?: string;
    id: number;
    name: string;
    company: string;
    isOfficeInstallation: boolean;
    databaseVersion: string;
    dialinPassword: string;
    accountEnabled: number;
}
