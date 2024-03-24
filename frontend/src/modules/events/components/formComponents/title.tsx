import { Grid, InputLabel, TextField } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import React, { useEffect, useRef } from 'react';
import useSetTitle from "../../store/hooks/eventDetails/useSetTitle";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";


const TitleField = () => {
    const eventStore = useEvent();
    const setTitle = useSetTitle();
    const disabled = eventStore.selectedEvent.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(eventStore.selectedEvent.title)
            inputRef.current.value = eventStore.selectedEvent.title;
    }, [eventStore.selectedEvent])

    const handleTitleChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(eventStore.selectedEvent.uuid, '/title', value)
            .then(() => {
                setTitle(eventStore.selectedEvent.uuid, value);
            })
            .catch(err => console.log(err));
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="title-input">Title</InputLabel>
            <TextField
                id="title-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handleTitleChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default TitleField;