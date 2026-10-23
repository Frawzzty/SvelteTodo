<script lang="ts">
    import { onMount } from 'svelte';
	import { render } from 'svelte/server';

    type TodoTask = {
        id: string;
        text: string;
        isDone: boolean;
        doneByDate: string;
    }

    let myNewTaskText:string = $state("");
    let myNewTaskDoneDate:string = $state("");

    let editId:string = $state("");

    let tasks = $state<TodoTask[]>([]);

    const baseUrl = "https://localhost:7190";
    const endPoint = "/api/tasks";
    
    //refrence original url that worked 'https://localhost:7190/api/tasks'
    
    onMount(async () => {
        await GetTasksAsync();
    });

    // GET TASKS
    async function GetTasksAsync(){
        try {
            const response = await fetch(baseUrl + endPoint);

            if (!response.ok)
                throw new Error(`Http Error: ${response.status}`);

            tasks = await response.json();
        }
        catch{
            console.log("Failed to load")
        }
    }

    // Create Task
    async function CreateTaskAsync(taskText:string, taskDate:string){
        let newTask:TodoTask = {
            id: "",
            text: taskText,
            isDone: false, // Default to false
            doneByDate: taskDate
        };

        try {
            const response = await fetch(baseUrl + endPoint, {
                method: 'POST',
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(newTask)
            })

            if (!response.ok)
                throw new Error('Could not add task');

            console.log('Added task successfully');

            // Clear inputs
            ClearNewTaskInputs();

            //tasks.push(newTask) // Add in local list. Not good? as it might not include formating happening in the backend etc
            await GetTasksAsync();
        }
        catch (error){{
            console.error(error);
        }}

    }

    // Update Task
    async function UpdateTaskAsync(todoTask: TodoTask)
    {
        try {
            const response = await fetch(baseUrl + endPoint + `/${todoTask.id}`, {
                method: 'PUT',
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(todoTask)
                }
            );

            if(!response.ok)
                throw new Error('Could not update task');

            console.log(`Updated Task. ID: ${todoTask.id}`)
            
        }
        catch (error){{
            console.error(error);
        }}
    }

    // DELETE Task by ID
    async function DeleteTaskAsync(id:string){
        try {
              const response = await fetch(`${baseUrl}${endPoint}/${id}`, { method: 'DELETE' }
            );
            
            if(!response.ok)
                throw new Error('Could not delete task')

            console.log('Delete task successfully')

            // Remove localy if api managed to delete it;
            tasks = tasks.filter(task => task.id !== id);
        }
        catch (error){
            console.error(error);
        }
    }

    function ClearNewTaskInputs(){
        myNewTaskText = "";
        myNewTaskDoneDate = "";
    }
</script>

<main>
    <h1>Svelte Todo</h1>

    <!-- ADD NEW TASK -->
    <div class="addTodoContainer">
        <div>
            <label for="addtodo">New task</label>
        </div>
        <div class="d-flex flex-column flex-gap">
            <input type="text" id="addtodo" class="textInput" bind:value={myNewTaskText}>
            <input type="Date" bind:value={myNewTaskDoneDate}/>
            <button onclick={() => CreateTaskAsync(myNewTaskText, myNewTaskDoneDate)} class="primaryButton">Add</button>
        </div>
    </div>

    <!-- DISPLAY TASKS -->
    <div class="todoTaskContainer">
        <h2>Tasks</h2>
        {#each tasks as task}
            {@render SnippetTodoTask(task)}
        {/each}
    </div>
</main>

{#snippet SnippetTodoTask (theTask:TodoTask)}
    <article class="todoTask">
    
    <!-- Edit mode -->
    {#if editId == theTask.id}

        <input type="text" bind:value={theTask.text}/>
        <input type="date" bind:value={theTask.doneByDate}/>
        <input type="checkbox" bind:checked={theTask.isDone}/>

        <button class="primaryButton" onclick={() => {
           editId = "";
        }}>Cancel</button>

        <button class="primaryButton" onclick={() => {
            UpdateTaskAsync(theTask);
            editId = "";
        }}>Update</button>

    <!-- Display mode -->
    {:else}
        <p>{theTask.text}</p>
        <p>Done: {theTask.isDone}</p>
        <p>Date: {theTask.doneByDate}</p>

        <button class="primaryButton" onclick={() => {
           editId = theTask.id
       }}>Edit</button>
    {/if}
     
        <button class="primaryButton" onclick={() => DeleteTaskAsync(theTask.id)}>Delete</button>
    </article>
{/snippet}


<style>
    :global(body) {
        background-color: var(--main-bg-color);
        color: white;
        margin: 0;
        padding: 0;
    }

    /* Main color */
    @property --main-bg-color {
        syntax: "<color>";
        inherits: false;
        initial-value: #202020;
    }
    /* Accent color */
    @property --accent-bg-color {
        syntax: "<color>";
        inherits: false;
        initial-value: #363636;
    }
    @property --accent-border-color {
        syntax: "<color>";
        inherits: false;
        initial-value: #6d6d6d;
    }
    /* Highlight color */
    @property --highlight-bg-color {
        syntax: "<color>";
        inherits: false;
        initial-value: #8d8d8d;
    }
    @property --highlight-border-color {
        syntax: "<color>";
        inherits: false;
        initial-value:  #b9b9b9;
    }

    main{
        padding: 1rem;
    }

    .d-flex{
        display: flex;
    }

    .flex-column{
        flex-direction: column;
    }
    .flex-gap{
        gap: 0.25rem;
    }
    
    .addTodoContainer{
        background: var(--accent-bg-color);

        border: 1px solid var(--accent-border-color);
        border-radius: 5px;

        padding: 1rem;
        margin-bottom: 1rem;
    }

    .todoTaskContainer{
        background: var(--accent-bg-color);

        border: 1px solid var(--accent-border-color);
        border-radius: 5px;

        padding: 1rem;
    }
    .todoTask{
        background: var(--highlight-bg-color);

        border: 1px solid var(--highlight-border-color);
        border-radius: 10px;

        padding: 1rem;
    }

    .primaryButton{
        background: #7d7f81;
        color: white;

        border-radius: 5px;

        padding: 0.25rem;
    }

    .textInput{
        padding-top: 0.5rem;
        padding-bottom: 0.5rem;
    }
</style>