import { IPerson } from "./contracts/IPerson";

export  class Person implements IPerson {
    id: number;
    name: string;
    picUrl: string;
    type: string;
    linkedin: string;
    twitter: string;
    medium: string;
    github: string;
    email: string;
    website: string;
}