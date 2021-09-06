import React from 'react'
import {useState,useEffect} from 'react'
import Ttss from '../TTS/Ttss'
import axios from "../../axios";
import Songs from './Songs'
import Song from './Song'

const TtsMaker = () => {

    const [ttss, setTtss] = useState([
        {
            id:1,
            name: 'Reggeli köszöntő',
            text: 'Jó reggelt kívánok minden kedves diáknak. Legyen szép napjuk!',
            used: false
        },
        {
            id:2,
            name: 'Fontos bejelentés',
            text: 'Fontos bejelentés! Kérem mindenki figyeljen ide! Ma az alábbi dolgok fognak történni:',
            used: false
        },
        {
            id:3,
            name: 'Tűzriadó',
            text: 'RIADÓ RIADÓ RIADÓ RIADÓ RIADÓ',
            used: false
        },
        {
            id:4,
            name: 'Probléma',
            text: 'A tanítás technikai okokból véget ért! Kérem mindenki menjen haza!',
            used: false
        }
    ])

    const [content,setContent] = useState('');

    const [fileName,setFileName] = useState('');
    
    const [tts,setTts]=useState([]);

    const deleteTts = (id) =>{
        setTtss(ttss.filter((ttss) => ttss.id!==id))
    }

    const addTts = () => {
        axios.post(`/File/PostTTSString/${content} & ${fileName}`)
        .then((response) => {
            console.log(response+" tts");
        })
        .catch((error) => {
            console.log(error);
        });
    }

    const onSubmit = (e) =>{
        e.preventDefault()
        addTts();
        //window.location.reload(false);
    }

    useEffect(() => {
        axios
        .get(`/File/GetAllTTSFiles/`)
        .then((response) => {
            const res = response.data;
            setTts(res);
            console.log(res+" files");
        })
        .catch((error) => {
            console.log(error);
        });
    },[]);

    const onDelete=(id)=>{
        axios.delete(`/File/DeleteFile/${id}`)
        .then((response) => {
            console.log(response);
          })
        .catch((error) => {
            console.log(error);
          });
      }

    return (
        <div className='container'>
            <h1>Felolvasandó szövegek</h1>
            <form className='form-control' onSubmit={onSubmit}>
                <h3>Új szöveg:</h3>
                    <input placeholder='Gépeld be a felolvasandó szöveget' onChange={e => setContent(e.target.value)}/>
                <h3>Fájlnév:</h3>
                    <input placeholder='Adj nevet a fájlnak' type='text' onChange={e => setFileName(e.target.value)}/><br/>
                <input type='submit' className='btn btn-block' value='Szöveg hozzáadása'/>

            <br/><br/>
            <h3>Felolvasható szövegek: </h3><br/>
            {
              tts.length > 0 ? 
              (
                  tts.map((file) => (
                      <Song
                          key={file}
                          song={file}
                          onDelete={onDelete}
                      />
                    ))
              ) 
              : 
              (
                  <p>Nincsenek feltöltött fájlok</p>
              )
            }
            </form>
        </div>
    )
}

export default TtsMaker
