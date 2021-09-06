import React, { useState, useEffect } from "react";

import axios from "../../axios";
import Ring from "./Ring";

const Rings = ({date}) => {
  //az összes csengetés ebben van
  const [ringsFromBackend, setRingsFromBackend] = useState([]);
  const [deleteClicked, setDeleteClicked] = useState(false);

  /*useEffect(() => renderrings(), [ringsFromBackend]);

  const renderrings = () => {
    const allrings = ringsFromBackend.map((ring) => (<Ring key={ring.id} ring={ring} onDelete={onDelete} />));
    return allrings;
  };*/

  //a megkapott mai nap dátumot olyanra alakítja, hogy a backend fel tudja dolgozni
  const dateFormatter = (date) => {
    let month = "" + (date.getMonth() + 1);
    let day = "" + date.getDate();
    let year = date.getFullYear();

    if (month.length < 2) month = "0" + month;
    if (day.length < 2) day = "0" + day;

    return [year, month, day].join("-");
  };

  /*//delete ring - még nincs használatban, de itt lehet
  const deleteRing = (id) => {
    //console.log('delete', id)
    setRingsFromBackend(ringsFromBackend.filter((ring) => ring.id !== id));
  };*/

  useEffect(() => {
    axios
      .get(`/Client/GetBellRingsForDay/${dateFormatter(date)}`)
      .then((response) => {
        const res = response.data;
        setRingsFromBackend(res);
        console.log(res);
      })
      .catch((error) => {
        console.log(error);
      });
  }, [date, deleteClicked]);

  const onDelete=(id)=>{
    axios.delete(`/Bellring/${id}`)
    .then((response) => {
        console.log(response);
        setDeleteClicked(!deleteClicked);
      })
    .catch((error) => {
        console.log(error);
      });
  }

  return (
    <div className="container">
      {ringsFromBackend.length > 0 ? 
      (
        ringsFromBackend.map((ring) => (
          <Ring
            key={ring.id}
            ring={ring}
            onDelete={onDelete}
          />
        ))
      ) 
      : 
      (
        <p>Ezen a napon nincs egy csengetés sem.</p>
      )}
    </div>
  );
};

export default Rings;
