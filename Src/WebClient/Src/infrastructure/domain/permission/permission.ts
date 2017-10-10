
export interface Permission {
    name: string;
    allowType: AccessRights;
    availability?: Availability;
}

export enum AccessRights {
    None = 0,
    Read = 1,
    Write = 2
}

export enum Availability {
    All = 1000,
    onlyShore = 1,
    // onlyShip = 2,
    onlyDualog = 3
}
