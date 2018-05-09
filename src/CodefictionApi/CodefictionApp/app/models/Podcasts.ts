import { IPodcast } from './contracts/IPodcast';
import { IRelation } from './contracts/IRelation';
import { IPerson } from './contracts/IPerson';

export class Podcast implements IPodcast {
  
  id: number;
  season: number;
  title: string;
  slug: string;
  soundcloudId: string;
  youtubeUrl: string;
  shortDescription: string;
  longDescription: string;
  guest: IPerson;
  attendees: IPerson[];
  tags: string[];
  relations: IRelation[];
  publishDate: Date;
}
