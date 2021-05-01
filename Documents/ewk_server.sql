--
-- PostgreSQL database cluster dump
--

-- Started on 2015-01-09 21:51:35

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE ewk_admin;
ALTER ROLE ewk_admin WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION PASSWORD 'md52e893b2ca95e1fd5eb405c58d193d327' VALID UNTIL 'infinity';
CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION PASSWORD 'md584eb25def26be059e7e18416fca5a81f';






--
-- Database creation
--

CREATE DATABASE ewk WITH TEMPLATE = template0 OWNER = ewk_admin;
REVOKE ALL ON DATABASE ewk FROM PUBLIC;
REVOKE ALL ON DATABASE ewk FROM ewk_admin;
GRANT ALL ON DATABASE ewk TO ewk_admin;
REVOKE ALL ON DATABASE template1 FROM PUBLIC;
REVOKE ALL ON DATABASE template1 FROM postgres;
GRANT ALL ON DATABASE template1 TO postgres;
GRANT CONNECT ON DATABASE template1 TO PUBLIC;


\connect ewk

SET default_transaction_read_only = off;

--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.0
-- Dumped by pg_dump version 9.4.0
-- Started on 2015-01-09 21:51:35

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 183 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2065 (class 0 OID 0)
-- Dependencies: 183
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

--
-- TOC entry 196 (class 1255 OID 16395)
-- Name: test_insert(character varying); Type: FUNCTION; Schema: public; Owner: ewk_admin
--

CREATE FUNCTION test_insert(email character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$BEGIN
	INSERT INTO accounts VALUES (DEFAULT, email, CURRENT_TIMESTAMP);
	RETURN;
END;$$;


ALTER FUNCTION public.test_insert(email character varying) OWNER TO ewk_admin;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 172 (class 1259 OID 16424)
-- Name: account; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE account (
    uid integer NOT NULL,
    email character varying NOT NULL,
    last_signed_date timestamp with time zone
);


ALTER TABLE account OWNER TO ewk_admin;

--
-- TOC entry 173 (class 1259 OID 16430)
-- Name: account_avatar; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE account_avatar (
    uid integer,
    auid integer,
    avatar_level integer,
    avatar_exp integer,
    keep_list json
);


ALTER TABLE account_avatar OWNER TO ewk_admin;

--
-- TOC entry 174 (class 1259 OID 16436)
-- Name: account_info; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE account_info (
    uid integer,
    first_signed_date timestamp with time zone,
    nick_name character varying(20),
    cash integer,
    money integer,
    word_count integer,
    word_deck_count integer,
    word_reference_count integer,
    rank_point integer,
    rank_win_count integer,
    rank_lose_count integer,
    rank_draw_count integer,
    attendance_count integer
);


ALTER TABLE account_info OWNER TO ewk_admin;

--
-- TOC entry 175 (class 1259 OID 16439)
-- Name: account_item; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE account_item (
    uid integer,
    keep_list json,
    buff_list json
);


ALTER TABLE account_item OWNER TO ewk_admin;

--
-- TOC entry 176 (class 1259 OID 16445)
-- Name: account_record_history; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE account_record_history (
    uid integer,
    total_record json,
    max_victory_count integer
);


ALTER TABLE account_record_history OWNER TO ewk_admin;

--
-- TOC entry 177 (class 1259 OID 16451)
-- Name: account_uid_seq; Type: SEQUENCE; Schema: public; Owner: ewk_admin
--

CREATE SEQUENCE account_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE account_uid_seq OWNER TO ewk_admin;

--
-- TOC entry 2066 (class 0 OID 0)
-- Dependencies: 177
-- Name: account_uid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ewk_admin
--

ALTER SEQUENCE account_uid_seq OWNED BY account.uid;


--
-- TOC entry 178 (class 1259 OID 16453)
-- Name: account_word; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE account_word (
    uid integer,
    registered_word json,
    referenced_word json
);


ALTER TABLE account_word OWNER TO ewk_admin;

--
-- TOC entry 179 (class 1259 OID 16459)
-- Name: avatar; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE avatar (
    uid integer NOT NULL,
    name character varying(30),
    description character varying(300),
    price integer
);


ALTER TABLE avatar OWNER TO ewk_admin;

--
-- TOC entry 180 (class 1259 OID 16462)
-- Name: avatar_uid_seq; Type: SEQUENCE; Schema: public; Owner: ewk_admin
--

CREATE SEQUENCE avatar_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE avatar_uid_seq OWNER TO ewk_admin;

--
-- TOC entry 2067 (class 0 OID 0)
-- Dependencies: 180
-- Name: avatar_uid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ewk_admin
--

ALTER SEQUENCE avatar_uid_seq OWNED BY avatar.uid;


--
-- TOC entry 181 (class 1259 OID 16464)
-- Name: item; Type: TABLE; Schema: public; Owner: ewk_admin; Tablespace: 
--

CREATE TABLE item (
    uid integer NOT NULL,
    name character varying(30),
    description character varying(300),
    price integer
);


ALTER TABLE item OWNER TO ewk_admin;

--
-- TOC entry 182 (class 1259 OID 16467)
-- Name: item_uid_seq; Type: SEQUENCE; Schema: public; Owner: ewk_admin
--

CREATE SEQUENCE item_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE item_uid_seq OWNER TO ewk_admin;

--
-- TOC entry 2068 (class 0 OID 0)
-- Dependencies: 182
-- Name: item_uid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: ewk_admin
--

ALTER SEQUENCE item_uid_seq OWNED BY item.uid;


--
-- TOC entry 1919 (class 2604 OID 16469)
-- Name: uid; Type: DEFAULT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account ALTER COLUMN uid SET DEFAULT nextval('account_uid_seq'::regclass);


--
-- TOC entry 1920 (class 2604 OID 16470)
-- Name: uid; Type: DEFAULT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY avatar ALTER COLUMN uid SET DEFAULT nextval('avatar_uid_seq'::regclass);


--
-- TOC entry 1921 (class 2604 OID 16471)
-- Name: uid; Type: DEFAULT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY item ALTER COLUMN uid SET DEFAULT nextval('item_uid_seq'::regclass);


--
-- TOC entry 2047 (class 0 OID 16424)
-- Dependencies: 172
-- Data for Name: account; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY account (uid, email, last_signed_date) FROM stdin;
1	test1@test.com	2015-01-03 01:03:35.439101+09
2	test1@test.com	2015-01-03 01:09:58.0569+09
3	test1@test.com	2015-01-03 01:10:14.799414+09
4	test1@test.com	2015-01-03 01:10:28.403225+09
\.


--
-- TOC entry 2048 (class 0 OID 16430)
-- Dependencies: 173
-- Data for Name: account_avatar; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY account_avatar (uid, auid, avatar_level, avatar_exp, keep_list) FROM stdin;
\.


--
-- TOC entry 2049 (class 0 OID 16436)
-- Dependencies: 174
-- Data for Name: account_info; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY account_info (uid, first_signed_date, nick_name, cash, money, word_count, word_deck_count, word_reference_count, rank_point, rank_win_count, rank_lose_count, rank_draw_count, attendance_count) FROM stdin;
\.


--
-- TOC entry 2050 (class 0 OID 16439)
-- Dependencies: 175
-- Data for Name: account_item; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY account_item (uid, keep_list, buff_list) FROM stdin;
\.


--
-- TOC entry 2051 (class 0 OID 16445)
-- Dependencies: 176
-- Data for Name: account_record_history; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY account_record_history (uid, total_record, max_victory_count) FROM stdin;
\.


--
-- TOC entry 2069 (class 0 OID 0)
-- Dependencies: 177
-- Name: account_uid_seq; Type: SEQUENCE SET; Schema: public; Owner: ewk_admin
--

SELECT pg_catalog.setval('account_uid_seq', 4, true);


--
-- TOC entry 2053 (class 0 OID 16453)
-- Dependencies: 178
-- Data for Name: account_word; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY account_word (uid, registered_word, referenced_word) FROM stdin;
\.


--
-- TOC entry 2054 (class 0 OID 16459)
-- Dependencies: 179
-- Data for Name: avatar; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY avatar (uid, name, description, price) FROM stdin;
\.


--
-- TOC entry 2070 (class 0 OID 0)
-- Dependencies: 180
-- Name: avatar_uid_seq; Type: SEQUENCE SET; Schema: public; Owner: ewk_admin
--

SELECT pg_catalog.setval('avatar_uid_seq', 1, false);


--
-- TOC entry 2056 (class 0 OID 16464)
-- Dependencies: 181
-- Data for Name: item; Type: TABLE DATA; Schema: public; Owner: ewk_admin
--

COPY item (uid, name, description, price) FROM stdin;
\.


--
-- TOC entry 2071 (class 0 OID 0)
-- Dependencies: 182
-- Name: item_uid_seq; Type: SEQUENCE SET; Schema: public; Owner: ewk_admin
--

SELECT pg_catalog.setval('item_uid_seq', 1, false);


--
-- TOC entry 1923 (class 2606 OID 16473)
-- Name: PK_account; Type: CONSTRAINT; Schema: public; Owner: ewk_admin; Tablespace: 
--

ALTER TABLE ONLY account
    ADD CONSTRAINT "PK_account" PRIMARY KEY (uid);


--
-- TOC entry 1929 (class 2606 OID 16475)
-- Name: PK_avatar; Type: CONSTRAINT; Schema: public; Owner: ewk_admin; Tablespace: 
--

ALTER TABLE ONLY avatar
    ADD CONSTRAINT "PK_avatar" PRIMARY KEY (uid);


--
-- TOC entry 1931 (class 2606 OID 16477)
-- Name: PK_item; Type: CONSTRAINT; Schema: public; Owner: ewk_admin; Tablespace: 
--

ALTER TABLE ONLY item
    ADD CONSTRAINT "PK_item" PRIMARY KEY (uid);


--
-- TOC entry 1925 (class 2606 OID 16479)
-- Name: account_avatar_auid_unique; Type: CONSTRAINT; Schema: public; Owner: ewk_admin; Tablespace: 
--

ALTER TABLE ONLY account_avatar
    ADD CONSTRAINT account_avatar_auid_unique UNIQUE (uid);


--
-- TOC entry 1927 (class 2606 OID 16481)
-- Name: account_item_auid_unique; Type: CONSTRAINT; Schema: public; Owner: ewk_admin; Tablespace: 
--

ALTER TABLE ONLY account_item
    ADD CONSTRAINT account_item_auid_unique UNIQUE (uid);


--
-- TOC entry 1932 (class 2606 OID 16482)
-- Name: account_avatar_auid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account_avatar
    ADD CONSTRAINT account_avatar_auid_fkey FOREIGN KEY (auid) REFERENCES avatar(uid);


--
-- TOC entry 1933 (class 2606 OID 16487)
-- Name: account_avatar_uid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account_avatar
    ADD CONSTRAINT account_avatar_uid_fkey FOREIGN KEY (uid) REFERENCES account(uid);


--
-- TOC entry 1934 (class 2606 OID 16492)
-- Name: account_info_uid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account_info
    ADD CONSTRAINT account_info_uid_fkey FOREIGN KEY (uid) REFERENCES account(uid);


--
-- TOC entry 1935 (class 2606 OID 16497)
-- Name: account_item_uid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account_item
    ADD CONSTRAINT account_item_uid_fkey FOREIGN KEY (uid) REFERENCES account(uid);


--
-- TOC entry 1936 (class 2606 OID 16502)
-- Name: account_record_history_uid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account_record_history
    ADD CONSTRAINT account_record_history_uid_fkey FOREIGN KEY (uid) REFERENCES account(uid);


--
-- TOC entry 1937 (class 2606 OID 16507)
-- Name: account_word_uid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: ewk_admin
--

ALTER TABLE ONLY account_word
    ADD CONSTRAINT account_word_uid_fkey FOREIGN KEY (uid) REFERENCES account(uid);


--
-- TOC entry 2064 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2015-01-09 21:51:35

--
-- PostgreSQL database dump complete
--

\connect postgres

SET default_transaction_read_only = off;

--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.0
-- Dumped by pg_dump version 9.4.0
-- Started on 2015-01-09 21:51:35

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 1990 (class 1262 OID 12135)
-- Dependencies: 1989
-- Name: postgres; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON DATABASE postgres IS 'default administrative connection database';


--
-- TOC entry 173 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 1993 (class 0 OID 0)
-- Dependencies: 173
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


--
-- TOC entry 172 (class 3079 OID 16384)
-- Name: adminpack; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;


--
-- TOC entry 1994 (class 0 OID 0)
-- Dependencies: 172
-- Name: EXTENSION adminpack; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';


--
-- TOC entry 1992 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2015-01-09 21:51:35

--
-- PostgreSQL database dump complete
--

\connect template1

SET default_transaction_read_only = off;

--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.0
-- Dumped by pg_dump version 9.4.0
-- Started on 2015-01-09 21:51:35

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 1989 (class 1262 OID 1)
-- Dependencies: 1988
-- Name: template1; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON DATABASE template1 IS 'default template for new databases';


--
-- TOC entry 172 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 1992 (class 0 OID 0)
-- Dependencies: 172
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


--
-- TOC entry 1991 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2015-01-09 21:51:36

--
-- PostgreSQL database dump complete
--

-- Completed on 2015-01-09 21:51:36

--
-- PostgreSQL database cluster dump complete
--

