
CREATE TABLE contact (
	contact_id uuid NOT NULL,
  contact_type varchar NULL,
	first_name varchar NULL,
	last_name varchar NOT NULL,
  middle_name varchar NULL,
  title varchar NULL,
	company varchar NOT NULL,
	description varchar NULL,
  photo varchar NULL,
  email varchar NULL,
  mobile varchar NULL,
  work_phone varchar NULL,
  home_phone varchar NULL,
  mailing_street varchar NULL,
  mailing_country varchar NULL,
  mailing_city varchar NULL,
  mailing_zipcode varchar NULL,
  mailing_state varchar NULL,
	CONSTRAINT contact_pk PRIMARY KEY (contact_id)
);
