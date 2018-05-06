import { IPerson } from './contracts/IPerson';
import { IRelation } from './contracts/IRelation';

export class Video  {
    id: number;
    title: string;
    slug: string;
    youtubeUrl: string;
    shortDescription: string;
    longDescription: string;
    attendees: IPerson[];
    tags: string[];
    relations: IRelation[];
    publishDate: Date;
    type: string;
}
